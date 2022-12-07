using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Areas.Rooms.Pages;

// TODO: Create data validation for user email / credit card
public class CustomerInformation : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerInformation(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public Customer? Input { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }
    
    public async Task<IActionResult> OnPostOnSubmitAsync()
    {
        _dbContext.Customers.Add(Input);

        await _dbContext.SaveChangesAsync();
        
        return RedirectToPage("Confirmation");
    }
    
    public async Task<IActionResult> OnPostOnCancelAsync()
    {
        return RedirectToPage("Reservation");
    }
}