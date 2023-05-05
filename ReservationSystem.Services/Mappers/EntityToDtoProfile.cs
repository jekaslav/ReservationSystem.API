using AutoMapper;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Services.Mappers
{
    public sealed class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<ChiefEntity, ChiefDto>();
            CreateMap<ClassroomEntity, ClassroomDto>();
            CreateMap<StudentEntity, StudentDto>();
            CreateMap<ReservationRequestEntity, ReservationRequestDto>();
        }
    }
}