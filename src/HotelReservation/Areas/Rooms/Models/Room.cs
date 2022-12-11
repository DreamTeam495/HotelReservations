using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Areas.Rooms.Models;

public class Room
{
    public int Id { get; set; }
    
    [Display(Name = "Name")]
    public string Name { get; set; }
    
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    
    [Display(Name = "Picture")]
    public byte[] Picture { get; set; }
    public bool IsActive { get; set; } = true;
    
    [Display(Name = "Description")]
    public string Description { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; }
}