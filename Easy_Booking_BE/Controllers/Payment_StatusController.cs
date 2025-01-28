using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Repositories;

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
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePayment_Status(int id, Payment_StatusModel payment_Status)
        {
            try
            {
                var ps = await _payment_StatusRepository.GetPayment_StatusById(id);
                if (ps != null)
                {
                    await _payment_StatusRepository.UpdatePayment_Status(id, payment_Status);
                    return Ok();    
                }
                return BadRequest("Payment_Status not found");                
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST: api/Payment_Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment_Status(Payment_StatusModel payment_Status)
        {
            try
            {
                var new_ps = await _payment_StatusRepository.AddPayment_Status(payment_Status);
                var nps = await _payment_StatusRepository.GetPayment_StatusById(new_ps);
                return nps == null ? NotFound() : Ok(nps);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Payment_Status/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePayment_Status(int id)
        {
            try
            {
                var ps = await _payment_StatusRepository.GetPayment_StatusById(id);
                if (ps != null)
                {
                    await _payment_StatusRepository.DeletePayment_Status(id);
                    return Ok();    
                }
                return BadRequest("Payment_Status not found");
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
