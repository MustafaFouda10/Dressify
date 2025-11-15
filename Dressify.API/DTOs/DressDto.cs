namespace Dressify.API.DTOs
{
    public class DressDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // Full URL (or relative path) exposed to clients
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; }
    }
}
