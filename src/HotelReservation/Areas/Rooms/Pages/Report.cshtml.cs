using HotelReservation.Areas.Rooms.Models; 
using HotelReservation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.EntityFrameworkCore; 
 
namespace HotelReservation.Areas.Rooms.Pages; 

//TODO: Make reporting more useful
[Authorize(Roles = "admin")]
public class Report : PageModel 
{ 
    private readonly ApplicationDbContext _dbContext; 
    
    public Report(ApplicationDbContext dbContext) 
    { 
        _dbContext = dbContext; 
    } 
     
    public List<Reservation> Reservations { get; set; }
 
    public async Task<IActionResult> OnGetAsync()
    {
        Reservations = await _dbContext.Reservations
            .Include(x => x.Customer)
            .Include(x => x.Room)
            .ToListAsync();

        return Page(); 
    } 
}