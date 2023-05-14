using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Services;
using ReservationSystem.UnitTests.Common;
using Xunit;

namespace ReservationSystem.UnitTests.Tests
{
    public class ReservationRequestServiceTests : TestCommandBase
    {
        private readonly ReservationRequestService _service;

        public ReservationRequestServiceTests()
        {
            var config = new MapperConfiguration(x => { x.CreateMap<ReservationRequestEntity, ReservationRequestDto>(); });
            var mapper = config.CreateMapper();

            _service = new ReservationRequestService(Context, mapper);
        }
        
        [Fact]
        public void GetAllReservationRequests_NotNull()
        {
            var result = _service.GetAllRequests(CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetReservationRequestById_NotNull()
        {
            var result = _service.GetRequestById(1, CancellationToken.None);

            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task CreateReservationRequest_PropertiesValuesCreatedCorrectly()
        {
            var requestDto = new ReservationRequestDto
            {
                Id = 7,
                StudentId = 5,
                ClassroomId = 2,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(1)
            };

            var result = await _service.Create(requestDto, CancellationToken.None);
    
            Assert.True(result);

            var updatedRequest = await Context.ReservationRequests
                .Where(x => x.Id == requestDto.Id)
                .FirstOrDefaultAsync();
            Assert.NotNull(updatedRequest);
            Assert.Equal(requestDto.StudentId, updatedRequest.StudentId);
            Assert.Equal(requestDto.ClassroomId, updatedRequest.ClassroomId);
        }
        
        [Fact] 
        public async Task UpdateReservationRequest_PropertiesValuesCreatedCorrectly()
        {
            var requestDto = new ReservationRequestDto
            {
                StudentId = 1,
                ClassroomId = 1,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(1),
            };
            
            var result = await _service.Update(7, requestDto, CancellationToken.None);
            
            Assert.True(result);
            
            var updatedRequest = await Context.ReservationRequests
                .AsNoTracking()
                .Where(x => x.Id == 7)
                .FirstOrDefaultAsync(CancellationToken.None);

            Assert.NotNull(updatedRequest);
            Assert.Equal(requestDto.StudentId, updatedRequest.StudentId);
            Assert.Equal(requestDto.ClassroomId, updatedRequest.ClassroomId);
            Assert.Equal(requestDto.StartTime, updatedRequest.StartTime);
            Assert.Equal(requestDto.EndTime, updatedRequest.EndTime);
        }
        
        [Fact]
        public async Task DeleteReservationRequest_RequestIsDeleted()
        {
            var deleteId = 7;
            var reservationRequestEntity = await Context.ReservationRequests
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.NotNull(reservationRequestEntity);
            
            var isDeleted = await _service.Delete(deleteId, CancellationToken.None);
            Assert.True(isDeleted);
            
            var deletedRequest = await Context.ReservationRequests
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.Null(deletedRequest);
        }

        [Fact]
        public async Task GetReservationRequestsForClassroom_CorrectlyNotNull()
        {
            var classroomId = 2;
            var chiefId = 1;
            
            var result = await _service.GetRequestsForClassroom(classroomId, chiefId, CancellationToken.None);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, rq => Assert.Equal(classroomId, rq.ClassroomId));
        }
        
        [Fact]
        public async Task UpdateReservationRequestStatus_UpdatesStatus_WhenReservationRequestExists()
        {
            var reservationRequestId = 7;
            var chiefId = 1;
            var newStatus = ReservationStatus.Approved;
            
            var reservationRequest = await Context.ReservationRequests
                .Where(x => x.Id == reservationRequestId)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            var result = await _service.UpdateReservationRequestStatus(reservationRequestId, chiefId, newStatus, CancellationToken.None);
            
            Assert.True(result);
            
            var updatedRequest = await Context.ReservationRequests
                .Where(x => x.Id == reservationRequestId)
                .FirstOrDefaultAsync(CancellationToken.None);

            Assert.NotNull(updatedRequest);
            Assert.Equal(newStatus, updatedRequest.Status);
        }
        
        [Fact]
        public async Task GetReservationRequestById_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetRequestById(negativeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task CreateReservationRequest_ThrowsArgumentNullExceptionForNullDto()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() 
                => _service.Create(null, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateReservationRequest_ThrowsArgumentExceptionForNegativeId()
        {
            var requestDto = new ReservationRequestDto
            {
                StudentId = 1,
                ClassroomId = 1,
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.Now.AddHours(1),
            };
            var negativeId = -7;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Update(negativeId, requestDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task DeleteReservationRequest_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -7;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Delete(negativeId, CancellationToken.None));
        }
    }
}