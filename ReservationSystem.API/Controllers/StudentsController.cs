using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystemAPI.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentService StudentService { get; }

        public StudentsController(IStudentService studentService)
        {
            StudentService = studentService;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents(CancellationToken cancellationToken)
        {
            var result = await StudentService.GetAllStudents(cancellationToken);

            return Ok(result);
        }

        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetStudentsById(int id, CancellationToken cancellationToken)
        {
            var result = await StudentService.GetStudentsById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost("students")]
        public async Task<IActionResult> Create([FromBody] StudentDto studentDto, CancellationToken cancellationToken)
        {
            var result = await StudentService.Create(studentDto, cancellationToken);

            return Ok(result);
        }

        [HttpPut("students/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentDto studentDto, CancellationToken cancellationToken)
        {
            var result = await StudentService.Update(id, studentDto, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("students/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await StudentService.Delete(id, cancellationToken);

            return Ok(result);
        }
    }        
}