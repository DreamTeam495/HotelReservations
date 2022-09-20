using HotelReservationAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationAPI.Contexts
{
    public class RoomContext : DbContext
    {
        public RoomContext(DbContextOptions<RoomContext> options) : base(options)
        {
        }

        public DbSet<Room>? ReservationItems { get; set; } = null;
    }
}
