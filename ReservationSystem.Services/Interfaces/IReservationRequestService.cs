using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Enums;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Services.Interfaces
{
    public interface IReservationRequestService
    {
        Task<IEnumerable<ReservationRequestDto>> GetAllRequests(CancellationToken cancellationToken);
        Task<ReservationRequestDto> GetRequestById(int id, CancellationToken cancellationToken);
        Task<bool> Create(ReservationRequestDto requestDto, CancellationToken cancellationToken);
        Task<bool> Update(int id, ReservationRequestDto requestDto, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task<List<ReservationRequestEntity>> GetRequestsForClassroom(int classroomId, int chiefId, CancellationToken cancellationToken);
        Task<bool> UpdateReservationRequestStatus(int reservationRequestId, int chiefId, ReservationStatus newStatus, CancellationToken cancellationToken);
    }
}