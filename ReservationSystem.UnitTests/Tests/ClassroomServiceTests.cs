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
    public class ClassroomServiceTests : TestCommandBase
    {
        private readonly ClassroomService _service;

        // public ClassroomServiceTests()
        // {
        //     var config = new MapperConfiguration(x => { x.CreateMap<ClassroomEntity, ClassroomDto>(); });
        //     var mapper = config.CreateMapper();
        //
        //     _service = new ClassroomService(Context, mapper);
        // }
        
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
        public async Task CreateClassroom_PropertiesValuesCreatedCorrectly()
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
        }

        [Fact] 
        public async Task UpdateClassroom_PropertiesValuesCreatedCorrectly()
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
        }
        
        [Fact]
        public async Task DeleteClassroom_ClassroomIsDeleted()
        {
            var deleteId = 2;
            var classroomEntity = await Context.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.NotNull(classroomEntity);
            
            var isDeleted = await _service.Delete(deleteId, CancellationToken.None);
            Assert.True(isDeleted);
            
            var deletedClassroom = await Context.Classrooms
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.Null(deletedClassroom);
        }
        
        [Fact]
        public async Task GetClassroomById_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetClassroomById(negativeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task CreateClassroom_ThrowsArgumentExceptionForNullLocation()
        {
            var classroomDto = new ClassroomDto
            {
                Id = 12,
                RoomNumber = 4,
                Capacity = 5,
                Location = null
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(classroomDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateClassroom_ThrowsArgumentExceptionForNegativeId()
        {
            var classroomDto = new ClassroomDto
            {
                RoomNumber = 5,
                Capacity = 6,
                Location = "UpdatedTestLocationName",
            };
            var negativeId = -2;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Update(negativeId, classroomDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task DeleteClassroom_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -2;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Delete(negativeId, CancellationToken.None));
        }
    }
}