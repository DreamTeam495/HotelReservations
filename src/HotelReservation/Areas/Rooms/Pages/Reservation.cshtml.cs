using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class Reservation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public Reservation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Room> Rooms { get; set; }
    
    [BindProperty]
    public Room? Input { get; set; }
    
    [BindProperty]
    [FromQuery]
    public DateTime StartDate { get; set; }
    
    [BindProperty]
    [FromQuery]
    public DateTime EndDate { get; set; }

    //TODO: Only return available rooms for given date range
    public async Task<IActionResult> OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
        return Page();
    }

    //TODO: Fix passing DateTime
    //TODO: Save Reservation to DB
    public async Task<IActionResult> OnPostOnReserveAsync(DateTime startDate, DateTime endDate)
    {
        var room = await _dbContext.Rooms.SingleAsync(x => x.Id == Input.Id);
        
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("CustomerInformation");
    }
}