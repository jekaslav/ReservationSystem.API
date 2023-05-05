using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Models;
using ReservationSystem.Services.Interfaces;

namespace ReservationSystemAPI.Controllers
{
    [ApiController]
    public class ReservationRequestController : ControllerBase
    {
        private  IReservationRequestService ReservationRequest { get; }

        public ReservationRequestController(IReservationRequestService reservationRequest)
        {
            ReservationRequest = reservationRequest;
        }
        
        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequests(CancellationToken cancellationToken,
            ReservationStatus? status = null)
        {
            var requests = await ReservationRequest.GetAllRequests(cancellationToken);
            
            if (status.HasValue)
            {
                requests = requests.Where(r => r.Status == status.Value);
            }
            
            return Ok(requests);
        }

        [HttpGet("requests/{id}")]
        public async Task<IActionResult> GetRequestById(int id, CancellationToken cancellationToken)
        {
            var result = await ReservationRequest.GetRequestById(id, cancellationToken);

            return Ok(result);
        }
        
        [HttpPost("requests")]
        public async Task<IActionResult> Create([FromBody] ReservationRequestDto requestDto, CancellationToken cancellationToken)
        {
            var result = await ReservationRequest.Create(requestDto, cancellationToken);

            return Ok(result);
        }
        
        [HttpPut("requests/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservationRequestDto requestDto, CancellationToken cancellationToken)
        {
            var result = await ReservationRequest.Update(id, requestDto, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("requests/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await ReservationRequest.Delete(id, cancellationToken);

            return Ok(result);
        }
        
        [HttpGet("{classroomId}")]
        public async Task<IActionResult> GetRequestsForClassroom(int classroomId, int chiefId, CancellationToken cancellationToken)
        {
            var requests = await ReservationRequest.GetRequestsForClassroom(chiefId, classroomId, cancellationToken);
            if (requests == null)
            {
                return BadRequest();
            }
            return Ok(requests);
        }
        
        [HttpPut("{classroomId}/{chiefId}/{reservationRequestId}/{newStatus}")]
        public async Task<IActionResult> UpdateReservationRequestStatus(int classroomId, int chiefId, int reservationRequestId, int newStatus, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(ReservationStatus), newStatus))
            {
                return BadRequest("Invalid ReservationStatus value");
            }

            var isSuccess = await ReservationRequest.UpdateReservationRequestStatus(reservationRequestId, chiefId, (ReservationStatus)newStatus, cancellationToken);
            
                return Ok();
        }
    }
}