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
    public class ClassroomServiceTests : TestCommandBase
    {
        private readonly ClassroomService _service;

        public ClassroomServiceTests()
        {
            var config = new MapperConfiguration(x => { x.CreateMap<ClassroomEntity, ClassroomDto>(); });
            var mapper = config.CreateMapper();

            _service = new ClassroomService(Context, mapper);
        }
        
        [Fact]
        public void GetAllClassrooms_NotNull()
        {
            var result = _service.GetAllClassrooms(CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetClassroomById_NotNull()
        {
            var result = _service.GetClassroomById(1, CancellationToken.None);

            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task<bool> CreateClassroom_PropertiesValuesCreatedCorrectly()
        {
            var classroomDto = new ClassroomDto
            {
                Id = 12,
                RoomNumber = 4,
                Capacity = 5,
                Location = "TestLocationName"
            };

            var result = await _service.Create(classroomDto, CancellationToken.None);
            Assert.True(result);

            var createdClassroom = await Context.Classrooms
                .Where(x => x.Id == classroomDto.Id)
                .FirstOrDefaultAsync();
            Assert.NotNull(createdClassroom);
            Assert.Equal(classroomDto.RoomNumber, createdClassroom.RoomNumber);
            Assert.Equal(classroomDto.Capacity, createdClassroom.Capacity);
            Assert.Equal(classroomDto.Location, createdClassroom.Location);

            return true;
        }

        [Fact] 
        public async Task<bool> UpdateClassroom_PropertiesValuesCreatedCorrectly()
        {
            var classroomDto = new ClassroomDto
            {
                RoomNumber = 5,
                Capacity = 6,
                Location = "UpdatedTestLocationName",
            };

            var result = await _service.Update(2, classroomDto, CancellationToken.None);
            
            Assert.True(result);

            var classroom = await Context.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == 2)
                .FirstOrDefaultAsync();
            
            Assert.NotNull(classroom);
            Assert.Equal(classroomDto.RoomNumber, classroom.RoomNumber);
            Assert.Equal(classroomDto.Capacity, classroom.Capacity);
            Assert.Equal(classroomDto.Location, classroom.Location);
            
            return true;
        }
        
        [Fact]
        public async Task<bool> DeleteClassroom_ClassroomIsDeleted()
        {
            var classroomEntity = await Context.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == 2)
                .FirstOrDefaultAsync();
            Assert.NotNull(classroomEntity);
            
            var isDeleted = await _service.Delete(classroomEntity.Id, CancellationToken.None);
            Assert.True(isDeleted);
            
            var deletedClassroom = await Context.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == classroomEntity.Id)
                .FirstOrDefaultAsync();
            Assert.Null(deletedClassroom);

            return true;
        }
    }
}