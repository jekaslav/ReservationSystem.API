using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ReservationDbContext ReservationDbContext;

        public ReservationService(ReservationDbContext dbContext)
        {
            ReservationDbContext = dbContext;
        }

        public async Task<bool> CreateReservation(int studentId, int classroomId, DateTimeOffset startTime,
            DateTimeOffset endTime, CancellationToken cancellationToken)
        {
            var existingReservation = ReservationDbContext.Reservations
                .Where(x => x.ClassroomId == classroomId)
                .Where(x => x.StartTime < endTime)
                .Where(x => x.EndTime > startTime)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingReservation is not null)
            {
                return false;
            }

            var reservation = new ReservationEntity
            {
                StudentId = studentId,
                ClassroomId = classroomId,
                StartTime = startTime,
                EndTime = endTime
            };

            ReservationDbContext.Reservations.Add(reservation);

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}