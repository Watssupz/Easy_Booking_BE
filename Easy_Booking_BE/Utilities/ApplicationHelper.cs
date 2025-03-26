using AutoMapper;
using Easy_Booking_BE.Data;
using Easy_Booking_BE.Models;
using EasyBooking.Data;
using Feature = EasyBooking.Data.Feature;

namespace Easy_Booking_BE.Utilities;

public class ApplicationHelper : Profile
{

    public ApplicationHelper()
    {
        CreateMap<Booking, BookingModel>().ReverseMap();
        CreateMap<Payment_Status, Payment_StatusModel>().ReverseMap();
        CreateMap<Room_Status, Room_StatusModel>().ReverseMap();
        CreateMap<Booking_Status, Booking_StatusModel>().ReverseMap();
        CreateMap<Feature, FeatureModel>().ReverseMap();
        CreateMap<Location, LocationModel>().ReverseMap();
        CreateMap<Room, RoomModel>()
            .ForMember(dest => dest.thumbnail, opt => opt.MapFrom(src => 
                src.thumbnail != null ? $"data:image/png;base64,{Convert.ToBase64String(src.thumbnail)}" : null
            ))
            .ReverseMap()
            .ForMember(dest => dest.thumbnail, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.thumbnail) && src.thumbnail.StartsWith("data:image/png;base64,")
                    ? Convert.FromBase64String(src.thumbnail.Replace("data:image/png;base64,", ""))
                    : null
            ));
        CreateMap<Media, MediaModel>().ReverseMap();
    }
    
}