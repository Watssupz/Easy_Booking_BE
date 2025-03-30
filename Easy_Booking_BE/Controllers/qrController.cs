using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Repositories;
using Final_Project_PRN221.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class qrController : ControllerBase
    {
        private readonly IQrGenerateRepository _qrGenerateRepository;

        public qrController(IQrGenerateRepository qrGenerateRepository)
        {
            _qrGenerateRepository = qrGenerateRepository;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQRCode(BankInfoDTO bankInfo)
        {
            var response = await _qrGenerateRepository.QrGenerate(bankInfo);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
    }
}
