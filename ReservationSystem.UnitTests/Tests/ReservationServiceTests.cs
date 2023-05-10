using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Services;
using ReservationSystem.UnitTests.Common;
using Xunit;

namespace ReservationSystem.UnitTests.Tests
{
    public class ReservationServiceTests : TestCommandBase
    {
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _service = new ReservationService(Context);
        }
        
        [Fact]
        public async Task <bool> CreateReservation_ReturnsTrue_WhenReservationIsValid()
        {
            var studentId = 5;
            var classroomId = 2;
            var startTime = DateTimeOffset.Now;
            var endTime = DateTimeOffset.Now.AddHours(1);
            
            var reservation = new ReservationEntity
            {
                StudentId = studentId,
                ClassroomId = classroomId,
                StartTime = startTime,
                EndTime = endTime
            };
            Context.Reservations.Add(reservation);

            await Context.SaveChangesAsync();

            var createdReservation = await Context.Reservations
                .Where(x => x.StudentId == studentId)
                .Where(x => x.ClassroomId == classroomId)
                .FirstOrDefaultAsync();
            Assert.NotNull(createdReservation);
            Assert.Equal(startTime, createdReservation.StartTime);
            Assert.Equal(endTime, createdReservation.EndTime);

            return true;
        }
        
    }
}