using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReservationSystem.Domain.Enums;

namespace ReservationSystem.Domain.Models
{
    public class ReservationRequestDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ReservationStatus Status { get; set; }
    }
}