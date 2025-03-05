using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MController : ControllerBase
    {
        private readonly IMediaRepository _mediaRepository;

        public MController(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        [Authorize]
        [HttpPost("Upload/{roomId}")]
        public async Task<IActionResult> Upload(int roomId, [FromForm] IFormFileCollection uploads)
        {
            var response = await _mediaRepository.CreateMediaByRoomId(roomId, uploads);
            return response.StatusCode == 200 ? Ok(response) : BadRequest(response);
        }
    }
}
