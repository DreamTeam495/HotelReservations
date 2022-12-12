using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

public class ManageType : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public ManageType(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Room> Rooms { get; set; }
    
    /// <summary>
    /// Retrieves all rooms currently in the database to be displayed to admin
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync()
    {
       Rooms = await _dbContext.Rooms.ToListAsync();
       return Page();
    }
}