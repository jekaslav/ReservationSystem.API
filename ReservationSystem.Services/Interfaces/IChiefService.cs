using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Services.Interfaces
{
    public interface IChiefService
    {
        Task<IEnumerable<ChiefDto>> GetAllChiefs(CancellationToken cancellationToken);
        Task<ChiefDto> GetChiefById(int id, CancellationToken cancellationToken);
        Task<bool> Create(ChiefDto chiefDto, CancellationToken cancellationToken);
        Task<bool> Update(int id, ChiefDto chiefDto, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task<bool> TakeControl(int classroomId, int chiefId, CancellationToken cancellationToken);
        Task<bool> ReleaseControl(int classroomId, int chiefId, CancellationToken cancellationToken);
    }
}