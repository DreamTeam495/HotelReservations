using HotelReservationAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationAPI.Contexts
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options)
        {
        }

        public DbSet<Reservation>? ReservationItems { get; set; } = null;
    }
}
