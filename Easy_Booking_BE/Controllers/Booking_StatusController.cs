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
            return Ok(await _booking_StatusRepository.GetAllBooking_Status());
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBookingStatus([FromBody] Booking_StatusModel model)
        {
            var result = await _booking_StatusRepository.CreateBooking_Status(model);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingStatusById(int id)
        {
            var result = await _booking_StatusRepository.GetBooking_StatusById(id);
            return result != null ? Ok(result) : NotFound();
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBookingStatus([FromRoute] int id, [FromBody] Booking_StatusModel model)
        {
            var bs = await _booking_StatusRepository.GetBooking_StatusById(id);
            if (bs != null)
            {
                if (id == model.booking_status_id)
                {
                    return Ok(await _booking_StatusRepository.UpdateBooking_Status(id, model));
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
        public async Task<IActionResult> DeleteBookingStatus(int id)
        {
            var bs = await _booking_StatusRepository.GetBooking_StatusById(id);
            if (bs.Data != null)
            {
                return Ok(await _booking_StatusRepository.DeleteBooking_Status(id));
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