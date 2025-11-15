using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressify.API.Models
{
    public class Dress
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        
        //public string ImageUrl { get; set; } = string.Empty;

        /*=== store only the relative file path (e.g. "images/abc.jpg") ===*/
        public string? ImagePath { get; set; }

        public bool IsAvailable { get; set; } = true;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
