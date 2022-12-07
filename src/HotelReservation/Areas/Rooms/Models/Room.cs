using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Areas.Rooms.Models;

public class Room
{
    public int Id { get; set; }
    
    [Display(Name = "Description")]
    public string Description { get; set; }
    
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    
    [Display(Name = "Picture")]
    public byte[] Picture { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<Reservation> Reservations { get; set; }
}