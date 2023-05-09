using System;

namespace ReservationSystem.Domain.Entities
{
    public class ReservationEntity
    {
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public StudentEntity Student { get; set; }
        public ClassroomEntity Classroom { get; set; }
    }
}