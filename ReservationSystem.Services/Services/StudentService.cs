﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Contexts;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.Services.Services
{
    public class StudentService : IStudentService
    {
        private ReservationDbContext ReservationDbContext { get; }
        
        private IMapper Mapper { get;} 
        
        public StudentService(ReservationDbContext context, IMapper mapper)
        {
            ReservationDbContext = context;
            Mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudents(CancellationToken cancellationToken)
        {
            var students = await ReservationDbContext.Students
                .AsNoTracking()
                .Select(x => Mapper.Map<StudentDto>(x))
                .ToListAsync(cancellationToken);

            if (students is null)
            {
                throw new NullReferenceException();
            }

            return students;
        }

        public async Task<StudentDto> GetStudentById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid ID");
            }
            
            var student = await ReservationDbContext.Students
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (student is null)
            {
                throw new NullReferenceException();
            }
            
            var result = Mapper.Map<StudentDto>(student);

            return result;
        }
        
        public async Task<bool> Create(StudentDto studentDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(studentDto.Name))
            {
                throw new ArgumentException();
            }
            
            if (string.IsNullOrWhiteSpace(studentDto.Email))
            {
                throw new ArgumentException();
            }
            
            var newStudent = new StudentEntity()
            {
                Id = studentDto.Id,
                Name = studentDto.Name,
                Email = studentDto.Email
            };
            
            ReservationDbContext.Students.Add(newStudent);
            
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }

        public async Task<bool> Update(int id, StudentDto studentDto, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid ID");
            }
            
            var studentToUpdate = await ReservationDbContext.Students
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (studentToUpdate is null)
            {
                throw new NullReferenceException();
            }
            
            studentToUpdate.Name = studentDto.Name ?? studentToUpdate.Name;
            studentToUpdate.Email = studentDto.Email ?? studentToUpdate.Email;

            await ReservationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new BadHttpRequestException("Invalid ID");
            }
            
            var studentToDelete = await ReservationDbContext.Students
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (studentToDelete is null)
            {
                throw new NullReferenceException();
            }
            
            ReservationDbContext.Students.Remove(studentToDelete);
            
            await ReservationDbContext.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}