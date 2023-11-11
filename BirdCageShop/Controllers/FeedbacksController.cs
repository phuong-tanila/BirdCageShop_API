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
using AutoMapper;
using DataTransferObjects;
using Repositories.Implements;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BirdCageShop.Controllers
{
    [Route("odata/[controller]")]
    public class FeedbacksController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _repo;

        public FeedbacksController(IFeedbackRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetAll()
        {
            var list = await _repo.GetAllAsync();

            if (list is null) return NotFound();

            return _mapper.Map<List<FeedbackDTO>>(list);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{cageId}")]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetByCage(Guid cageId)
        {
            var list = await _repo.GetByCageAsync(cageId);

            if (list is null) return NotFound();

            return _mapper.Map<List<FeedbackDTO>>(list);
        }

        // POST: api/OrderDetails/5
        [HttpPost("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutOrderDetail(Guid id, [FromBody] FeedbackDTO model)
        {
            if (!ModelState.IsValid || id != model.Id) return BadRequest("Invalid format");

            OrderDetail? orderDetail = await _repo.GetAsync(model.Id);
            if (orderDetail == null) return NotFound();

            if (orderDetail.PostDate != null) return BadRequest("Feedback already exist");

            orderDetail.Rating = model.Rating;
            orderDetail.Content = model.Content;
            try
            {
                await _repo.AddAsync(orderDetail);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
