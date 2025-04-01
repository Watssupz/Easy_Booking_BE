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

        [Authorize]
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var response = await _bookingsRepository.MyBookingAsync();
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("confirm-booking/{booking_id}")]
        public async Task<IActionResult> ConfirmBooking(int booking_id)
        {
            var response = await _bookingsRepository.ConfirmBookingAsync(booking_id);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
        
        [HttpPost("cancel-booking/{booking_id}")]
        public async Task<IActionResult> CancelBooking(int booking_id)
        {
            var response = await _bookingsRepository.CancelBookingAsync(booking_id);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("get-bookings")]
        public async Task<IActionResult> GetBookings()
        {
            var response = await _bookingsRepository.GetListBookingByRooms();
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("check-in/{booking_id}")]
        public async Task<IActionResult> CheckIn(int booking_id)
        {
            var response = await _bookingsRepository.CheckInBooking(booking_id);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [HttpGet("check-out/{booking_id}")]
        public async Task<IActionResult> CheckOut(int booking_id)
        {
            var response = await _bookingsRepository.CheckOutBooking(booking_id);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("get-bookingstatus")]
        public async Task<IActionResult> GetBookingsStatus()
        {
            var response = await _bookingsRepository.GetBookingCountByStatus();
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
    }
}
