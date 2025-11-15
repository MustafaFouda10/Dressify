namespace Dressify.API.DTOs
{
    public class CreateReservationDto
    {
        public int DressId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
    }
}
