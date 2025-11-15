using Dressify.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dressify.API.Data
{
    public class DressifyDbContext : DbContext
    {
        public DressifyDbContext(DbContextOptions<DressifyDbContext> options)
            : base(options) { }

        public DbSet<Dress> Dresses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
