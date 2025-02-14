using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RFController : ControllerBase
    {
        private readonly IRoom_FeatureRepository _room_feature_repository;

        public RFController(IRoom_FeatureRepository room_feature_repository)
        {
            _room_feature_repository = room_feature_repository;
        }

        [HttpPut("UpdateFeature")]
        public async Task<IActionResult> UpdateRoomFeatureIds([FromBody] UpdateRoomFeatureModel model)
        {
            var result = await _room_feature_repository.UpdateRoomFeatureIds(model);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

    }
}
