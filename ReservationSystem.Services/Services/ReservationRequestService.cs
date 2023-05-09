using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services.Services
{
    public class ReservationRequestService : IReservationRequestService
    {
        private ReservationDbContext ReservationDbContext { get; }
        
        private IMapper Mapper { get; }

        public ReservationRequestService(ReservationDbContext context, IMapper mapper)
        {
            ReservationDbContext = context;
            Mapper = mapper;
        }
        
        public async Task<IEnumerable<ReservationRequestDto>> GetAllRequests(CancellationToken cancellationToken)
        {
            var requests = await ReservationDbContext.ReservationRequests
                .AsNoTracking()
                .Select(x => Mapper.Map<ReservationRequestDto>(x))
                .ToListAsync(cancellationToken);
            
            if (requests is null)
            {
                throw new NullReferenceException();
            }
            
            return requests;
        }

        public async Task<ReservationRequestDto> GetRequestById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid ID");
            }
            
            var request = await ReservationDbContext.ReservationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (request is null)
            {
                throw new ArgumentException($"Reservation request with ID {id} does not exist.");
            }

            var result = Mapper.Map<ReservationRequestDto>(request);

            return result;
        }
        
        public async Task<int> Create(ReservationRequestDto requestDto, CancellationToken cancellationToken)
        {
            if (requestDto is null)
            {
                throw new ArgumentNullException(nameof(requestDto));
            }

            var newRequest = new ReservationRequestEntity()
            {
                ClassroomId = requestDto.ClassroomId,
                StudentId = requestDto.StudentId,
                StartTime = requestDto.StartTime,
                EndTime = requestDto.EndTime
            };
            
            ReservationDbContext.ReservationRequests.Add(newRequest);
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return newRequest.Id;
        }
        
        public async Task<bool> Update(int id, ReservationRequestDto requestDto, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid reservation request ID");
            }

            var requestToUpdate = await ReservationDbContext.ReservationRequests
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            requestToUpdate.ClassroomId = requestDto.ClassroomId;
            requestToUpdate.StudentId = requestDto.StudentId;
            requestToUpdate.StartTime = requestDto.StartTime;
            requestToUpdate.EndTime = requestDto.EndTime;

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid reservation request ID");
            }
            
            var requestToDelete = await ReservationDbContext.ReservationRequests
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            ReservationDbContext.ReservationRequests.Remove(requestToDelete);
            
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
        
        public async Task<List<ReservationRequestEntity>> GetRequestsForClassroom(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            var chiefClassroom = await ReservationDbContext.ChiefClassrooms
                .Where(x => x.ClassroomId == classroomId)
                .Where(x => x.ChiefId == chiefId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (chiefClassroom is null)
            {
                return null;
            }
            
            var requests = await ReservationDbContext.ReservationRequests
                .Where(x => x.ClassroomId == classroomId)
                .ToListAsync(cancellationToken);
            
            return requests;
        }

        public async Task<bool> UpdateReservationRequestStatus(int reservationRequestId, int chiefId, ReservationStatus newStatus, CancellationToken cancellationToken)
        {
            var request = await ReservationDbContext.ReservationRequests
                .FirstOrDefaultAsync(x => x.Id == reservationRequestId, cancellationToken);
            if (request is null)
            {
                return false;
            }

            var chiefClassroom = await ReservationDbContext.ChiefClassrooms
                .Where(x => x.ClassroomId == request.ClassroomId)
                .Where(x => x.ChiefId == chiefId)
                .FirstOrDefaultAsync(cancellationToken);
            if (chiefClassroom is null)
            {
                return false;
            }

            var reservationService = new ReservationService(ReservationDbContext);
            var success = await reservationService.CreateReservation(request.StudentId, request.ClassroomId,
                request.StartTime, request.EndTime, cancellationToken);
            if (!success)
            {
                return false;
            }

            request.Status = newStatus;

            await using var transaction = await ReservationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var reservationCreated = await reservationService.CreateReservation(request.StudentId, request.ClassroomId, 
                    request.StartTime, request.EndTime, cancellationToken);
                if (!reservationCreated) 
                {
                    return false;
                }
  
                request.Status = newStatus;
                await ReservationDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}