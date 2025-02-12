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
    public class FController : ControllerBase
    {
        private readonly IFeatureRepository _featureRepository;

        public FController(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllFeature()
        {
            return Ok(await _featureRepository.GetAllFeaturesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeatureById(int id)
        {
            var result = await _featureRepository.GetFeatureByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }
        
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateFeature([FromBody] FeatureModel model)
        {
            var result = await _featureRepository.CreateFeatureAsync(model);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateFeature(int id, [FromBody] FeatureModel model)
        {
            var f = await _featureRepository.GetFeatureByIdAsync(id);
            if (f != null)
            {
                if (id == model.feature_id)
                {
                    return Ok(await _featureRepository.UpdateFeatureAsync(id, model));
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
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var f = await _featureRepository.GetFeatureByIdAsync(id);
            if (f.Data != null)
            {
                return Ok(await _featureRepository.DeleteFeatureAsync(id));
            }
            return NotFound(
                new BaseDataResponse<object>(
                    statusCode: 404,
                    message: Constants.NOT_FOUND
                )
            );
        }
        
    }
}
