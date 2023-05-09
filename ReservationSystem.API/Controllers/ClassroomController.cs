using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.API.Controllers
{
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private IClassroomService ClassroomService { get; }

        public ClassroomController(IClassroomService classroomService)
        {
            ClassroomService = classroomService;
        }

        [HttpGet("classrooms")]
        public async Task<IActionResult> GetAllClassrooms(CancellationToken cancellationToken)
        {
            var result = await ClassroomService.GetAllClassrooms(cancellationToken);

            return Ok(result);
        }

        [HttpGet("classrooms/{id}")]
        public async Task<IActionResult> GetClassroomById(int id, CancellationToken cancellationToken)
        {
            var result = await ClassroomService.GetClassroomById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost("classrooms")]
        public async Task<IActionResult> Create([FromBody] ClassroomDto classroomDto, CancellationToken cancellationToken)
        {
            var result = await ClassroomService.Create(classroomDto, cancellationToken);

            return Ok(result);
        }

        [HttpPut("classrooms/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClassroomDto classroomDto, CancellationToken cancellationToken)
        {
            var result = await ClassroomService.Update(id, classroomDto, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("classrooms/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await ClassroomService.Delete(id, CancellationToken.None);

            return Ok(result);
        }
    }
}