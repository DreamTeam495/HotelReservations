using HotelReservation.Areas.Rooms.Models;
using HotelReservation.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelReservation.Areas.Rooms.Pages;

[Authorize(Roles = "admin")]
public class AddTypeModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public AddTypeModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public Room? Input { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var file = Request.Form.Files[0];
        
        using (var dataStream = new MemoryStream())
        {
            await file.CopyToAsync(dataStream);
            Input.Picture = dataStream.ToArray();
        }
        
        _dbContext.Rooms.Add(Input);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage();
    }
}