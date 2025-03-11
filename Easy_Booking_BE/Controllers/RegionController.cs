using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Easy_Booking_BE.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public RegionController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [HttpGet("provinces")]
        public async Task<IActionResult> GetAllProvinces()
        {
            var response = await _regionRepository.GetProvincesAsync();
            return response.StatusCode == 200 ? Ok(response) : NotFound();
        }

        [HttpGet("districts/{province_id}")]
        public async Task<IActionResult> GetAllDistrictByProvince(int province_id)
        {
            var response = await _regionRepository.GetDistrictAsync(province_id);
            return response.StatusCode == 200 ? Ok(response) : NotFound();
        }

        [HttpGet("wards/{district_id}")]
        public async Task<IActionResult> GetAllWardsByDistrict(int district_id)
        {
            var response = await _regionRepository.GetWardAsync(district_id);
            return response.StatusCode == 200 ? Ok(response) : NotFound();
        }
    }
}
