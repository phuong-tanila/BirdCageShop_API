using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Internal;

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

        // GET: odata/Orders
        // Get all for customer
        [HttpGet("odata/[Controller]/customer/history")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersCustomer()
        {
            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "");
            if (accessToken == null) return BadRequest("Invalid access token");

            try
            {
                var user = await _accRepo.FindByTokenAsync(accessToken);
                if (user == null) return NotFound("Invalid access token");

                Customer? customer = await _cusRepo.GetByAccountIdAsync(user.Id);
                if (customer == null) return NotFound("Invalid access token");

                return Ok(await _repo.GetAllByCustomerAsync(customer.Id));
            }
            catch (Exception)
            {
                return BadRequest("Something wrong");
            }
        }

        // PUT: odata/Orders/5
        //[EnableQuery]
        //public async Task<IActionResult> PutOrder(Guid id, Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: odata/Orders
        [EnableQuery]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> PostAsync([FromBody] Order model)
        {
            if (!ModelState.IsValid || model is null || model.OrderDetails.Count == 0)
                return BadRequest("Invalid format");

            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "");
            if (accessToken == null) return BadRequest("Invalid access token");

            try
            {
                // Check valid order
                #region
                var user = await _accRepo.FindByTokenAsync(accessToken);
                if (user == null) return NotFound("Invalid access token");

                Customer? customer = await _cusRepo.GetByAccountIdAsync(user.Id);
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
                List<OrderDetail> list = (List<OrderDetail>)model.OrderDetails;
                var dictionary = new Dictionary<Guid, int>();
                if (Contains(list)) return BadRequest("Invalid order detail");

                foreach (OrderDetail i in list)
                {
                    Cage cage = await _cageRepo.GetNonDeletedCageByIdAsync(i.CageId);
                    if (cage is null || cage.InStock < i.Quantity)
                    {
                        return BadRequest("Invalid cage");
                    }
                    // Add orderDetail
                    var newOrderDrtails = new Dictionary<Guid, int>();
                }

                await _repo.AddAsync(model);
            }
            catch (Exception)
            {
                return BadRequest("Create fail");
            }

            return Created(model);
        }

        private bool Contains(List<OrderDetail> list)
        {
            foreach (var i1 in list)
            {
                foreach (var i2 in list)
                {
                    if (i1.CageId == i2.CageId) { return true; }
                }
            }
            return false;
        }

        // DELETE: odata/Orders/5
        //[EnableQuery]
        //public async Task<IActionResult> DeleteOrder(Guid id)
        //{
        //    if (_context.Orders == null)
        //    {
        //        return NotFound();
        //    }
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        
    }
}
