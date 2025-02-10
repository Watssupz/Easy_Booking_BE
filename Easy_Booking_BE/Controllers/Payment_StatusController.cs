using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PSController : ControllerBase
    {
        private readonly IPayment_StatusRepository _payment_StatusRepository;
        
        public PSController(IPayment_StatusRepository payment_StatusRepository)
        {
            _payment_StatusRepository = payment_StatusRepository;
        }

        // GET: api/Payment_Status
        [HttpGet("All")]
        public async Task<IActionResult> GetAllPayment_Status()
        {
            try
            {
                return Ok(await _payment_StatusRepository.GetAllPayment_Status());
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/Payment_Status/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment_StatusById(int id)
        {
            var ps = await _payment_StatusRepository.GetPayment_StatusById(id);
            return ps == null ? NotFound() : Ok(ps);
        }

        // PUT: api/Payment_Status/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePayment_Status(int id, Payment_StatusModel payment_Status)
        {
            var ps = await _payment_StatusRepository.GetPayment_StatusById(id);
            if (ps != null)
            {
                if (id == payment_Status.payment_id)
                {
                    // await _payment_StatusRepository.UpdatePayment_Status(id, payment_Status);
                    return Ok(await _payment_StatusRepository.UpdatePayment_Status(id, payment_Status));
                }
                return BadRequest(
                    new BaseDataResponse<object>
                    (
                        statusCode: 400,
                        message: Constants.NOT_MATCH
                    )
                );
            }
            return NotFound(
                new BaseDataResponse<object>
                (
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                )
            );
        }

        // POST: api/Payment_Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment_Status(Payment_StatusModel payment_Status)
        {
            if (payment_Status == null)
            {
                return BadRequest
                (
                    new BaseDataResponse<object>
                    (
                        statusCode: 400,
                        message: Constants.NOT_FOUND
                    )
                );
            }
            var result = await _payment_StatusRepository.AddPayment_Status(payment_Status);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest
            (
                new BaseDataResponse<object>
                (
                    statusCode: 400,
                    message: Constants.ALREADY_EXSIST
                )
            );
        }

        // DELETE: api/Payment_Status/5
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePayment_Status(int id)
        {
            var ps = await _payment_StatusRepository.GetPayment_StatusById(id);
            if (ps.Data != null)
            {
                return Ok(await _payment_StatusRepository.DeletePayment_Status(id));
            }
            return BadRequest(
                new BaseDataResponse<object>
                (
                    statusCode: 400,
                    message: Constants.NOT_FOUND
                )
            );
        }

    }
}
