using System;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;

namespace ReservationSystem.UnitTests.Common
{
    public class DataFiller
    {
        public static ReservationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ReservationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ReservationDbContext(options);
            context.Database.EnsureCreated();

            context.Chiefs.Add(new ChiefEntity
            {
                Id = 1,
                Name = "ChiefName",
                Email = "ChiefEmail"
            });

            context.Classrooms.Add(new ClassroomEntity
            {
                Id = 2,
                RoomNumber = 3,
                Capacity = 4,
                Location = "LocationName",
            });

            context.Students.Add(new StudentEntity
            {
                Id = 5,
                Name = "StudentName",
                Email = "StudentEmail"
            });

            context.ChiefClassrooms.Add(new ChiefClassroomEntity
            {
                Id = 7,
                ChiefId = 1,
                ClassroomId = 2
            });

            context.ReservationRequests.Add(new ReservationRequestEntity
            {
                Id = 7,
                StudentId = 5,
                ClassroomId = 2,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(1),
            });

            context.SaveChanges();
            return context;
        }

        public static void Delete(ReservationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}