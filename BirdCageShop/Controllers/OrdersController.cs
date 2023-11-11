using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using DataTransferObjects.CageDTOs;

namespace BirdCageShop.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly IOrderRepository _repo;
        private readonly IAccountRepository _accRepo;
        private readonly ICustomerRepository _cusRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly ICageRepository _cageRepo;

        public OrdersController(IOrderRepository repo,
            IAccountRepository accRepo,
            ICustomerRepository cusRepo,
            IVoucherRepository voucherRepo,
            ICageRepository cageRepo)
        {
            _repo = repo;
            _accRepo = accRepo;
            _cusRepo = cusRepo;
            _voucherRepo = voucherRepo;
            _cageRepo = cageRepo;
        }

        // Get all for staff/manager
        // GET: odata/Orders
        [EnableQuery]
        //[Authorize (Roles = "Staff, Manager")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await _repo.GetAllAsync());
        }

        // Get order for staff/manager
        // GET: odata/Orders/5
        [EnableQuery]
        //[Authorize(Roles = "Staff, Manager")]
        public async Task<ActionResult<Order>> Get(Guid key)
        {
            var model = await _repo.GetAsync(key);

            if (model == null) return NotFound();

            return Ok(model);
        }

        // Get all for customer
        // GET: odata/Orders
        [HttpGet("odata/[controller]/history")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersCustomer()
        {
            try
            {
                Customer? customer = await GetCustomerFromTokenAsync();
                if (customer == null) return NotFound("Invalid access token");

                return Ok(await _repo.GetAllByCustomerAsync(customer.Id));
            }
            catch (Exception)
            {
                return BadRequest("Something wrong");
            }
        }

        // POST: odata/Orders
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> PostAsync([FromBody] Order model)
        {
            if (!ModelState.IsValid || model is null || model.OrderDetails.Count == 0)
                return BadRequest("Invalid format");
            try
            {
                // Check valid order
                #region
                Customer? customer = await GetCustomerFromTokenAsync();
                if (customer == null) return NotFound("Invalid access token");

                if (customer.Id != model.CustomerId) return BadRequest("Invalid customer");

                if (model.VoucherId != null)
                {
                    bool isValid = await _voucherRepo.IsValidAsync((Guid)model.VoucherId);
                    if (!isValid) return BadRequest("Invalid voucher");

                    Voucher? voucher = await _voucherRepo.GetByIdAsync((Guid)model.VoucherId);
                    if (customer.Point < voucher!.ConditionPoint)
                        return BadRequest("Not enough points to use the voucher");
                }
                #endregion

                // Check valid order detail
                #region
                List<OrderDetail> list = (List<OrderDetail>)model.OrderDetails;
                var newList = new Dictionary<Guid, int>();
                foreach (OrderDetail i in list)
                {
                    // Check quantity inStock
                    Cage cage = await _cageRepo.GetNonDeletedCageByIdAsync(i.CageId);
                    if (cage is null || cage.InStock < i.Quantity)
                    {
                        return BadRequest($"Invalid cage {i.CageId} inventory quantity");
                    }
                    // Check cage already been added
                    try { newList.TryAdd(i.CageId, i.Quantity); }
                    catch (Exception) { return BadRequest($"Cage {i.CageId} already been added"); }
                }
                #endregion
                // Update status cage

                // Create order
                await _repo.AddAsync(model);
            }
            catch (Exception)
            {
                return BadRequest("Create fail");
            }

            return Created(model);
        }

        // PUT: odata/Orders/delivering
        [HttpPut("odata/[controller]/delivering")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delivering(Guid id)
        {
            Order? order = await _repo.GetAsync(id);
            if (order is null) return NotFound();

            if (order.Status != (int)OrderStatus.Processing) return BadRequest("Invalid status order");
            try
            {
                await _repo.DeliveringAsync(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: odata/Orders/completed
        [HttpPut("odata/[controller]/completed")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Completed(Guid id)
        {
            Order? order = await _repo.GetAsync(id);
            if (order is null) return NotFound();

            if (order.Status != (int)OrderStatus.Delivering) return BadRequest("Invalid status order");
            try
            {
                await _repo.CompleteAsync(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        // PUT: odata/Orders/cancel
        [HttpPut("odata/[controller]/cancel")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            Customer? customer = await GetCustomerFromTokenAsync();
            if (customer == null) return NotFound("Invalid access token");

            Order? order = await _repo.GetAsync(id);
            if (order is null) return NotFound();

            if (customer.Id != order.CustomerId) return BadRequest("Invalid order id");

            if (order.Status != (int)OrderStatus.Processing) return BadRequest("Invalid status order");
            try
            {
                await _repo.CancelAsync(order);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        private async Task<Customer?> GetCustomerFromTokenAsync()
        {
            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "");
            if (accessToken == null) return null!;

            try
            {
                var user = await _accRepo.FindByTokenAsync(accessToken);
                if (user == null) return null!;

                Customer? customer = await _cusRepo.GetByAccountIdAsync(user.Id);
                return customer!;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
