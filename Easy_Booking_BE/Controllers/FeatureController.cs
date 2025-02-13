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
            var result = await _featureRepository.GetAllFeaturesAsync();
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeatureById(int id)
        {
            var result = await _featureRepository.GetFeatureByIdAsync(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateFeature([FromBody] FeatureModel model)
        {
            var result = await _featureRepository.CreateFeatureAsync(model);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateFeature(int id, [FromBody] FeatureModel model)
        {
            var result = await _featureRepository.UpdateFeatureAsync(id, model);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var result = await _featureRepository.DeleteFeatureAsync(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }
    }
}