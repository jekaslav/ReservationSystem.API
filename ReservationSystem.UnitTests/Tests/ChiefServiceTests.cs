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
    public class ChiefServiceTests : TestCommandBase
    {
        private readonly ChiefService _service;

        public ChiefServiceTests()
        {
            var config = new MapperConfiguration(x => { x.CreateMap<ChiefEntity, ChiefDto>(); });
            var mapper = config.CreateMapper();

            _service = new ChiefService(Context, mapper);
        }

        [Fact]
        public void GetAllChiefs_NotNull()
        {
            var result = _service.GetAllChiefs(CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetChiefById_NotNull()
        {
            var result = _service.GetChiefById(1, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateChief_PropertiesValuesCreatedCorrectly()
        {
            var chiefDto = new ChiefDto
            {
                Id = 2,
                Name = "TestChief",
                Email = "test_chief@example.com"
            };
    
            var result = await _service.Create(chiefDto, CancellationToken.None);
    
            Assert.True(result);

            var chief = await Context.Chiefs
                .Where(x => x.Id == chiefDto.Id)
                .FirstOrDefaultAsync();
    
            Assert.NotNull(chief);
            Assert.Equal(chiefDto.Email, chief.Email);
            Assert.Equal(chiefDto.Name, chief.Name);
        }

        [Fact]
        public async Task UpdateChief_PropertiesValuesCreatedCorrectly()
        {
            var chiefDto = new ChiefDto
            {
                Name = "UpdatedChiefName",
                Email = "updated_chief_email@example.com"
            };

            var result = await _service.Update(1, chiefDto, CancellationToken.None);
            
            Assert.True(result);

            var chief = await Context.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync();
            
            Assert.NotNull(chief);
            Assert.Equal(chiefDto.Email, chief.Email);
            Assert.Equal(chiefDto.Name, chief.Name);
        }

        [Fact]
        public async Task DeleteChief_ChiefIsDeleted()
        {
            var deleteId = 1;
            var chiefEntity = await Context.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.NotNull(chiefEntity);

            var isDeleted = await _service.Delete(deleteId, CancellationToken.None);
            Assert.True(isDeleted);

            var deletedChief = await Context.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.Null(deletedChief);
        }

        [Fact]
        public async Task TakeControlChief_RelationshipValueCreatedCorrectly()
        {
            var classroomId = 2;
            var chiefId = 1;

            var result = await _service.TakeControl(classroomId, chiefId, CancellationToken.None);
            
            Assert.True(result);

            var chiefClassroom = await Context.ChiefClassrooms
                .Where(x => x.ClassroomId == classroomId)
                .Where(x => x.ChiefId == chiefId)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.NotNull(chiefClassroom);
        }

        [Fact]
        public async Task ReleaseControlChief_RelationshipValueDeleted()
        {
            var classroomId = 2;
            var chiefId = 1;
            
            var addedChiefClassroom = await Context.ChiefClassrooms
                .Where(x => x.ClassroomId == classroomId)
                .Where(x => x.ChiefId == chiefId)
                .FirstOrDefaultAsync();
            Assert.NotNull(addedChiefClassroom);

            var result = await _service.ReleaseControl(classroomId, chiefId, CancellationToken.None);
            Assert.True(result);

            var deletedChiefClassroom = await Context.ChiefClassrooms
                .Where(x => x.ClassroomId == classroomId)
                .Where(x => x.ChiefId == chiefId)
                .FirstOrDefaultAsync();

            Assert.Null(deletedChiefClassroom);
        }
        
        [Fact]
        public async Task GetChiefById_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetChiefById(negativeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task CreateChief_ThrowsArgumentExceptionForInvalidName()
        {
            var chiefDto = new ChiefDto
            {
                Id = 2,
                Name = null,
                Email = "test_chief@example.com"
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(chiefDto, CancellationToken.None));
        }

        [Fact]
        public async Task CreateChief_ThrowsArgumentExceptionForInvalidEmail()
        {
            var chiefDto = new ChiefDto
            {
                Id = 2,
                Name = "TestChief",
                Email = null
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(chiefDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateChief_ThrowsArgumentExceptionForNegativeId()
        {
            var chiefDto = new ChiefDto
            {
                Name = "UpdatedChiefName",
                Email = "updated_chief_email@example.com"
            };
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Update(negativeId, chiefDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task DeleteChief_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Delete(negativeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task TakeControlChief_ThrowsArgumentExceptionForNegativeClassroomOrChiefId()
        {
            var negativeClassroomId = -2;
            var negativeChiefId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.TakeControl(negativeClassroomId, negativeChiefId, CancellationToken.None));
        }
        
        [Fact]
        public async Task ReleaseControlChief_ThrowsArgumentExceptionForNegativeClassroomOrChiefId()
        {
            var negativeClassroomId = -2;
            var negativeChiefId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.ReleaseControl(negativeClassroomId, negativeChiefId, CancellationToken.None));
        }
    }
}