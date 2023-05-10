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
        public async Task<bool> CreateChief_PropertiesValuesCreatedCorrectly()
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

            return true;
        }

        [Fact]
        public async Task<bool> UpdateChief_PropertiesValuesCreatedCorrectly()
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
            
            return true;
        }

        [Fact]
        public async Task<bool> DeleteChief_ChiefIsDeleted()
        {
            var chiefEntity = await Context.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync();
            Assert.NotNull(chiefEntity);
            
            var isDeleted = await _service.Delete(chiefEntity.Id, CancellationToken.None);
            Assert.True(isDeleted);
            
            var deletedChief = await Context.Chiefs
                .AsNoTracking()
                .Where(x => x.Id == chiefEntity.Id)
                .FirstOrDefaultAsync();
            Assert.Null(deletedChief);

            return true;
        }

        [Fact]
        public async Task<bool> TakeControlChief_RelationshipValueCreatedCorrectly()
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

            return true;
        }

        [Fact]
        public async Task<bool> ReleaseControlChief_RelationshipValueDeleted()
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

            return true;
        }
    }
}