using System.Collections.Generic;
using System.Linq;

namespace ReservationSystem.Domain.Entities
{
    public class ChiefEntity
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        public IEnumerable<ChiefClassroomEntity> ChiefClassrooms { get; set; }
        
        // public int? GetAssignedClassroomId(int classroomId)
        // {
        //     var chiefClassroom = ChiefClassrooms.FirstOrDefault(x => x.ClassroomId == classroomId);
        //     return chiefClassroom?.ClassroomId;
        // }
    }
}