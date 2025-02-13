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
    public class RSController : ControllerBase
    {
        private readonly IRoom_StatusRepository _roomStatusRepository;

        public RSController(IRoom_StatusRepository roomStatusRepository)
        {
            _roomStatusRepository = roomStatusRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllRoom_Status()
        {
            var result = await _roomStatusRepository.GetAllRoom_Status();
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom_Status([FromBody] Room_StatusModel model)
        {
            var result = await _roomStatusRepository.CreateRoom_Status(model);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom_StatusById(int id)
        {
            var result = await _roomStatusRepository.GetRoom_StatusById(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateRoom_Status(int id, [FromBody] Room_StatusModel model)
        {
            var result = await _roomStatusRepository.UpdateRoom_Status(id, model);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRoom_Status(int id)
        {
            var result = await _roomStatusRepository.DeleteRoom_Status(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }
    }
}