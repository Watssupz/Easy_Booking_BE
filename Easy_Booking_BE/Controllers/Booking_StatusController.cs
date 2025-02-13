using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Repositories;
using EasyBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BSController : ControllerBase
    {
        private readonly IBooking_StatusRepository _booking_StatusRepository;

        public BSController(IBooking_StatusRepository booking_StatusRepository)
        {
            _booking_StatusRepository = booking_StatusRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllBookingStatus()
        {
            var result = await _booking_StatusRepository.GetAllBooking_Status();
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBookingStatus([FromBody] Booking_StatusModel model)
        {
            var result = await _booking_StatusRepository.CreateBooking_Status(model);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingStatusById(int id)
        {
            var result = await _booking_StatusRepository.GetBooking_StatusById(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBookingStatus([FromRoute] int id, [FromBody] Booking_StatusModel model)
        {
            var bs = await _booking_StatusRepository.UpdateBooking_Status(id, model);
            return bs.StatusCode == 200 ? Ok(bs) : NotFound(bs);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBookingStatus(int id)
        {
            var bs = await _booking_StatusRepository.DeleteBooking_Status(id);
            return bs.StatusCode == 200 ? Ok(bs) : NotFound(bs);
        }
    }
}