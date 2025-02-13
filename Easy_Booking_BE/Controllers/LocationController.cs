using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllLocations()
        {
            var result = await _locationRepository.GetLocationsAsync();
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _locationRepository.GetLocationByIdAsync(id);
            return location.StatusCode == 200 ? Ok(location) : NotFound(location);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationModel location)
        {
            var result = await _locationRepository.CreateLocationAsync(location);
            return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateLocations(int id, [FromBody] LocationModel models)
        {
            var result = await _locationRepository.UpdateLocationAsync(id, models);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var result = await _locationRepository.DeleteLocationAsync(id);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchLocation(LocationModel model)
        {
            var result = await _locationRepository.SearchLocationAsync(model);
            return result.StatusCode == 200 ? Ok(result) : NotFound(result);
        }
    }
}