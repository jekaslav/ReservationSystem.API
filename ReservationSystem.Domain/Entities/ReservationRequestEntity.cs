using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ReservationSystem.Domain.Enums;

namespace ReservationSystem.Domain.Entities
{
    public class ReservationRequestEntity
    {
        public ReservationRequestEntity()
        {
            Status = ReservationStatus.Pending;
        }

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        
        // [JsonIgnore]
        public StudentEntity Student { get; set; }
        // [JsonIgnore]
        public ClassroomEntity Classroom { get; set; }
    }
}