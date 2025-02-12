using AutoMapper;
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
    }
    
}