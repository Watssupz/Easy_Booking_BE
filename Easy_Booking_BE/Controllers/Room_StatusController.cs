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
            return Ok(await _roomStatusRepository.GetAllRoom_Status());
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom_Status([FromBody] Room_StatusModel model)
        {
            var result = await _roomStatusRepository.CreateRoom_Status(model);

            if (result.StatusCode == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom_StatusById(int id)
        {
            var result = await _roomStatusRepository.GetRoom_StatusById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateRoom_Status(int id, [FromBody] Room_StatusModel model)
        {
            var rs = await _roomStatusRepository.GetRoom_StatusById(id);
            if (rs != null)
            {
                if (id == model.room_status_id)
                {
                    return Ok(await _roomStatusRepository.UpdateRoom_Status(id, model));
                }

                return BadRequest(
                    new BaseDataResponse<object>(
                        statusCode: 400,
                        message: Constants.NOT_MATCH
                    )
                );
            }

            return NotFound(
                new BaseDataResponse<object>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                )
            );
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRoom_Status(int id)
        {
            var rs = await  _roomStatusRepository.GetRoom_StatusById(id);
            if (rs.Data != null)
            {
                return Ok(await _roomStatusRepository.DeleteRoom_Status(id));
            }

            return BadRequest(
                new BaseDataResponse<object>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                )
            );
        }
    }
}