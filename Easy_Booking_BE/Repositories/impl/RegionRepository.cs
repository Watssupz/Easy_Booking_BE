using Easy_Booking_BE.Data;
using Easy_Booking_BE.Data.Constants;
using Easy_Booking_BE.Models;
using Easy_Booking_BE.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Easy_Booking_BE.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly EasyBookingBEContext _context;

    public RegionRepository(EasyBookingBEContext context)
    {
        _context = context;
    }

    public async Task<BaseDataResponse<List<ProvinceModel>>> GetProvincesAsync()
    {
        var list_provinces = await _context.Regions
            .Where(r => r.province_name != null && r.province_id != null)
            .GroupBy(r => new { r.province_id, r.province_name })
            .Select(g => new ProvinceModel
            {
                province_id = g.Key.province_id,
                province_name = g.Key.province_name!
            })
            .ToListAsync();

        return new BaseDataResponse<List<ProvinceModel>>(
            statusCode: 200,
            message: Constants.FOUND,
            data: list_provinces
        );
    }

    public async Task<BaseDataResponse<List<DistrictModel>>> GetDistrictAsync(int province_id)
    {
        var list_district = await _context.Regions
            .Where(r => r.province_id == province_id && r.district_name  != null && r.district_id != null)
            .GroupBy(r => new { r.district_id, r.district_name  })
            .Select(g => new DistrictModel
            {
                district_id = g.Key.district_id,
                district_name = g.Key.district_name!
            })
            .ToListAsync();
        return new BaseDataResponse<List<DistrictModel>>(
            statusCode: 200,
            message: Constants.FOUND,
            data: list_district
        );
    }

    public async Task<BaseDataResponse<List<WardModel>>> GetWardAsync(int district_id)
    {
        var list_wards = await _context.Regions
            .Where(r => r.district_id == district_id && r.ward_name != null && r.ward_id != null)
            .GroupBy(r => new { r.ward_id, r.ward_name })
            .Select(g => new WardModel
            {
                ward_id = g.Key.ward_id,
                ward_name = g.Key.ward_name!
            })
            .ToListAsync();

        return new BaseDataResponse<List<WardModel>>(
            statusCode: 200,
            message: Constants.FOUND,
            data: list_wards
        );
    }
}