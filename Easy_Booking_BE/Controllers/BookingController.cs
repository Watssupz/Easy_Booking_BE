using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Repositories;
using EasyBooking.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookingController : ControllerBase
    {
        private readonly IBookingsRepository _bookingsRepository;
        public bookingController(IBookingsRepository repo)
        {
            _bookingsRepository = repo;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllBookings()
        {
            return Ok(await _bookingsRepository.GetAllBookingsAsync());
        }

        [HttpGet("get-schedule/{room_id}")]
        public async Task<IActionResult> GetBookingByRoomId(int room_id)
        {
            var response = await _bookingsRepository.GetBookingByRoomIdAsync(room_id);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateModel booking)
        {
            var response = await _bookingsRepository.AddBookingAsync(booking);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
        
    }
}
