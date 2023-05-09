using System.Collections.Generic;

namespace ReservationSystem.Domain.Entities
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<ReservationRequestEntity> ReservationRequestList { get; set; }
        public IEnumerable<ReservationEntity> ReservationList { get; set; }
    }
}