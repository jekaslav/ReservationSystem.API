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
    public class ChiefService : IChiefService
    {
        private ReservationDbContext ReservationDbContext { get; }
        
        private IMapper Mapper { get; }

        public ChiefService(ReservationDbContext context, IMapper mapper)
        {
            ReservationDbContext = context;
            Mapper = mapper;
        }

        public async Task<IEnumerable<ChiefDto>> GetAllChiefs(CancellationToken cancellationToken)
        {
            var chiefs = await ReservationDbContext.Chiefs
                .AsNoTracking()
                .Select(x => Mapper.Map<ChiefDto>(x))
                .ToListAsync(cancellationToken);

            if (!chiefs.Any())
            {
                throw new NullReferenceException();
            }

            return chiefs;
        }

        public async Task<ChiefDto> GetChiefsById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var chiefs = await ReservationDbContext.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (chiefs == null)
            {
                throw new KeyNotFoundException();
            }

            var result = Mapper.Map<ChiefDto>(chiefs);

            return result;
        }

        public async Task<bool> Create(ChiefDto chiefDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(chiefDto.Name))
            {
                throw new ArgumentException();
            }
            
            var newChief = new ChiefEntity()
            {
                Id = chiefDto.Id,
                Name = chiefDto.Name,
                Email = chiefDto.Email
            };
            
            ReservationDbContext.Chiefs.Add(newChief);

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Update(int id, ChiefDto chiefDto, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var chiefToUpdate = await ReservationDbContext.Chiefs
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (chiefToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            chiefToUpdate.Name = chiefDto.Name ?? chiefToUpdate.Name;
            chiefToUpdate.Email = chiefDto.Email ?? chiefDto.Email;

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var chiefToDelete = await ReservationDbContext.ChiefClassrooms
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (chiefToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            ReservationDbContext.ChiefClassrooms.Remove(chiefToDelete);

            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TakeControl(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            if (classroomId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            if (chiefId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var chief = await ReservationDbContext.Chiefs.FindAsync(chiefId);
            var classroomToControl = await ReservationDbContext.Classrooms.FindAsync(classroomId);

            if (chief == null || classroomToControl == null)
            {
                return false;
            }

            var chiefClassroom = new ChiefClassroomEntity
            {
                Chief = chief,
                Classroom = classroomToControl
            };

            ReservationDbContext.ChiefClassrooms.Add(chiefClassroom);
            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> ReleaseControl(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            if (classroomId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            if (chiefId <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid ID");
            }
            
            var chiefClassroom = await ReservationDbContext.ChiefClassrooms
                .FirstOrDefaultAsync(x => x.ClassroomId == classroomId && x.ChiefId == chiefId, cancellationToken);
            
            if (chiefClassroom == null)
            {
                return false;
            }
            
            ReservationDbContext.ChiefClassrooms.Remove(chiefClassroom);
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}