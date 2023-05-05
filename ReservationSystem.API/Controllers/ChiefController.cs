using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystem.API.Controllers
{
    [ApiController]
    public class ChiefController : ControllerBase
    {
        private IChiefService ChiefService { get; }

        public ChiefController(IChiefService chiefService)
        {
            ChiefService = chiefService;
        }
        
        [HttpGet("chiefs")]
        public async Task<IActionResult> GetAllChiefs(CancellationToken cancellationToken)
        {
            var result = await ChiefService.GetAllChiefs(cancellationToken);

            return Ok(result);
        }
        
        [HttpGet("chiefs/{id}")]
        public async Task<IActionResult> GetClassroomsById(int id, CancellationToken cancellationToken)
        {
            var result = await ChiefService.GetChiefsById(id, cancellationToken);

            return Ok(result);
        }
        
        [HttpPost("chiefs")]
        public async Task<IActionResult> Create([FromBody] ChiefDto chiefDto, CancellationToken cancellationToken)
        {
            var result = await ChiefService.Create(chiefDto, cancellationToken);

            return Ok(result);
        }
        
        [HttpPut("chiefs/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChiefDto chiefDto, CancellationToken cancellationToken)
        {
            var result = await ChiefService.Update(id, chiefDto, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("chiefs/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await ChiefService.Delete(id, cancellationToken);

            return Ok(result);
        }
        
        [HttpPost]
        [Route("{classroomId}/chiefs/{chiefId}")]
        public async Task<IActionResult> TakeControl(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            var result = await ChiefService.TakeControl(classroomId, chiefId, cancellationToken);

            return Ok(result);
        }
        
        [HttpDelete("{classroomId}/chiefs/{chiefId}")]
        public async Task<IActionResult> ReleaseControl(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            var result = await ChiefService.ReleaseControl(classroomId, chiefId, cancellationToken);

            return Ok(result);
        } 
    }
}