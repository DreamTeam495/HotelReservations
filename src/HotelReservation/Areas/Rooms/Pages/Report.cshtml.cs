using HotelReservation.Areas.Rooms.Models; 
using HotelReservation.Data; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.EntityFrameworkCore; 
 
namespace HotelReservation.Areas.Rooms.Pages; 
 
public class Report : PageModel 
{ 
    private readonly ApplicationDbContext _dbContext; 
 
 
    public Report(ApplicationDbContext dbContext) 
    { 
        _dbContext = dbContext; 
    } 
     
    public IEnumerable<Room> Rooms { get; set; } 
 
    public async Task<IActionResult> OnGetAsync() 
    {
        Rooms = await _dbContext.Rooms
            .ToListAsync(); 
         
        return Page(); 
    } 
}