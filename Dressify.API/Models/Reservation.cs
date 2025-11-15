using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dressify.API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int DressId { get; set; }
        public Dress? Dress { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

        
    public enum ReservationStatus 
    { 
        Pending, Confirmed, Collected, Canceled 
    }

}
