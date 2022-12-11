using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using HotelReservation.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class RoomSelect : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public RoomSelect(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Room> Rooms { get; set; }

    [BindProperty]
    public Room? Input { get; set; }

    [BindProperty]
    public DateTime StartDate { get; set; }
    
    [BindProperty]
    public DateTime EndDate { get; set; }



    //TODO: Only return available rooms for given date range
    public async Task<IActionResult> OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
        StartDate = HttpContext.Session.Get<DateTime>("_StartDate");
        EndDate = HttpContext.Session.Get<DateTime>("_EndDate");
        return Page();
    }
    
    public async Task<IActionResult> OnPostOnReserveAsync()
    {
        var room = await _dbContext.Rooms.SingleAsync(x => x.Id == Input.Id);

        await _dbContext.SaveChangesAsync();

        return RedirectToPage("CustomerInformation", new { Input.Id });
    }
}