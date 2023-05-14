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
    public class StudentServiceTests : TestCommandBase
    {
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            var config = new MapperConfiguration(x => { x.CreateMap<StudentEntity, StudentDto>(); });
            var mapper = config.CreateMapper();

            _service = new StudentService(Context, mapper);
        }
        
        [Fact]
        public void GetAllStudents_NotNull()
        {
            var result = _service.GetAllStudents(CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetStudentById_NotNull()
        {
            var result = _service.GetStudentById(1, CancellationToken.None);

            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task CreateStudent_PropertiesValuesCreatedCorrectly()
        {
            var studentDto = new StudentDto
            {
                Id = 2,
                Name = "TestStudent",
                Email = "test_student@example.com"
            };
    
            var result = await _service.Create(studentDto, CancellationToken.None);
    
            Assert.True(result);

            var student = await Context.Students
                .Where(x => x.Id == studentDto.Id)
                .FirstOrDefaultAsync();
    
            Assert.NotNull(student);
            Assert.Equal(studentDto.Email, student.Email);
            Assert.Equal(studentDto.Name, student.Name);
        }
        
        [Fact]
        public async Task UpdateStudent_PropertiesValuesCreatedCorrectly()
        {
            var studentDto = new StudentDto
            {
                Name = "UpdatedStudentName",
                Email = "updated_student_email@example.com"
            };

            var result = await _service.Update(5, studentDto, CancellationToken.None);
            
            Assert.True(result);

            var student = await Context.Students
                .AsNoTracking()
                .Where(x => x.Id == 5)
                .FirstOrDefaultAsync();
            
            Assert.NotNull(student);
            Assert.Equal(studentDto.Email, student.Email);
            Assert.Equal(studentDto.Name, student.Name);
        }
        
        [Fact]
        public async Task DeleteStudent_StudentIsDeleted()
        {
            var deleteId = 5;
            var studentEntity = await Context.Students
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.NotNull(studentEntity);
            
            var isDeleted = await _service.Delete(deleteId, CancellationToken.None);
            Assert.True(isDeleted);
            
            var deletedStudent = await Context.Students
                .AsNoTracking()
                .Where(x => x.Id == deleteId)
                .FirstOrDefaultAsync();
            Assert.Null(deletedStudent);
        }
        
        [Fact]
        public async Task GetStudentById_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -1;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetStudentById(negativeId, CancellationToken.None));
        }
        
        [Fact]
        public async Task CreateStudent_ThrowsArgumentExceptionForNullOrWhiteSpaceNameOrEmail()
        {
            var studentDto = new StudentDto
            {
                Id = 2,
                Name = " ",
                Email = null
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(studentDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task UpdateStudent_ThrowsArgumentExceptionForNegativeId()
        {
            var studentDto = new StudentDto
            {
                Name = "UpdatedStudentName",
                Email = "updated_student_email@example.com"
            };
            var negativeId = -5;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Update(negativeId, studentDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task DeleteStudent_ThrowsArgumentExceptionForNegativeId()
        {
            var negativeId = -5;
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Delete(negativeId, CancellationToken.None));
        }
    }
}