namespace ReservationSystem.Domain.Entities
{
    public class ChiefClassroomEntity
    {
        public int Id { get; set; }
        public int ChiefId { get; set; }
        public int ClassroomId { get; set; }
        public ChiefEntity Chief { get; set; }
        public ClassroomEntity Classroom { get; set; }
    }
}