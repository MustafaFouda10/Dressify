using Dressify.API.Models;

namespace Dressify.API.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int DressId { get; set; }
        public string DressName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
