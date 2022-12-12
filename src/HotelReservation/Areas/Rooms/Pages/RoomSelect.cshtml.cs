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



    /// <summary>
    /// Displays rooms stored in Rooms database, while retrieving dates from session to be used.
    /// </summary>
    //TODO: Only return available rooms for given date range
    public async Task<IActionResult> OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
        StartDate = HttpContext.Session.Get<DateTime>("_StartDate");
        EndDate = HttpContext.Session.Get<DateTime>("_EndDate");
        return Page();
    }
    
    /// <summary>
    /// On reserve button select, query parameter is sent rather than session in case user decides to choose a different room.
    /// </summary>
    /// <returns>User goes to CustomerInformation page, and has a Id value passed as a parameter.</returns>
    public async Task<IActionResult> OnPostOnReserveAsync()
    {
        return RedirectToPage("CustomerInformation", new { Input.Id });
    }
}