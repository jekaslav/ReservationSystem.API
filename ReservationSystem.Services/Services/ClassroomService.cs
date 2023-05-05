using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services.Services
{
    public class ClassroomService : IClassroomService
    {
        private ReservationDbContext ReservationDbContext { get; }
        private IMapper Mapper { get; }
        
        public ClassroomService(ReservationDbContext context, IMapper mapper)
        {
            ReservationDbContext = context;
            Mapper = mapper;
        }
        
        public async Task<IEnumerable<ClassroomDto>> GetAllClassrooms(CancellationToken cancellationToken)
        {
            var classrooms = await ReservationDbContext.Classrooms
                .AsNoTracking()
                .Select(x => Mapper.Map<ClassroomDto>(x))
                .ToListAsync(cancellationToken);

            if (!classrooms.Any())
            {
                throw new NullReferenceException();
            }

            return classrooms;
        }
        
        public async Task<ClassroomDto> GetClassroomsById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var classroom = await ReservationDbContext.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (classroom == null)
            {
                throw new KeyNotFoundException();
            }
            
            var result = Mapper.Map<ClassroomDto>(classroom);

            return result;
        }
        
        public async Task<bool> Create(ClassroomDto classroomDto, CancellationToken cancellationToken)
        {
            var newClassroom = new ClassroomEntity()
            {
                Id = classroomDto.Id,
                RoomNumber = classroomDto.RoomNumber,
                Capacity = classroomDto.Capacity,
                Location = classroomDto.Location
            };
            
            ReservationDbContext.Classrooms.Add(newClassroom);
            
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
        
        public async Task<bool> Update(int id, ClassroomDto classroomDto, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var classroomToUpdate = await ReservationDbContext.Classrooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (classroomToUpdate == null)
            {
                throw new KeyNotFoundException();
            }
            
            classroomToUpdate.RoomNumber = classroomDto.RoomNumber ?? classroomToUpdate.RoomNumber;
            classroomToUpdate.Capacity = classroomDto.Capacity ?? classroomToUpdate.Capacity;
            classroomToUpdate.Location = classroomDto.Location ?? classroomToUpdate.Location;

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var classroomToDelete = await ReservationDbContext.Classrooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (classroomToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            ReservationDbContext.Classrooms.Remove(classroomToDelete);
            
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}