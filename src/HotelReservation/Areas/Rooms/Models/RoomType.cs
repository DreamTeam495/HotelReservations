using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Areas.Rooms.Models;

public class RoomType
{
    public int Id { get; set; }
    
    [Display(Name = "Description")]
    public string Description { get; set; }
    
    [Display(Name = "Price")]
    public decimal Price { get; set; }
    
    [Display(Name = "Picture")]
    public byte[] Picture { get; set; }
}