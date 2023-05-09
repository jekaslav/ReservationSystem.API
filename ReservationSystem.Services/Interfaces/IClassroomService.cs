using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Services.Interfaces
{
    public interface IClassroomService
    {
        Task<IEnumerable<ClassroomDto>> GetAllClassrooms(CancellationToken cancellationToken);
        Task<ClassroomDto> GetClassroomById(int id, CancellationToken cancellationToken);
        Task<bool> Create(ClassroomDto classroomDto, CancellationToken cancellationToken);
        Task<bool> Update(int id, ClassroomDto classroomDto, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}