using System.Text.RegularExpressions; 
using Microsoft.Playwright; 
using Microsoft.Playwright.NUnit; 
 
namespace tests; 
 
[Parallelizable(ParallelScope.Self)] 
[TestFixture] 
public class FullRun : PageTest 
{ 
    [Test] 
    public async Task HomepageHasDreamHotelInTitleAndFullReservationProcess() 
    { 
        await Page.GotoAsync("https://localhost:7022"); 
 
        // Expect a title "to contain" a substring. 
        await Expect(Page).ToHaveTitleAsync(new Regex("Dream Hotel")); 
 
        // create a locator 
        var reservation = Page.GetByRole(AriaRole.Link, new() { Name = "Reservations" }); 
 
        // Expect an attribute "to be strictly equal" to the value. 
        await Expect(reservation).ToHaveAttributeAsync("href", "/Rooms/DateSelect"); 
 
        // Click the get started link. 
        await reservation.ClickAsync(); 
 
        // Expects the URL to contain intro. 
        await Expect(Page).ToHaveURLAsync(new Regex(".*DateSelect"));

        // Click submit after date select 
        await Page.Locator("button").Nth(1).ClickAsync();
         
        await Expect(Page).ToHaveURLAsync(new Regex(".*Reservation")); 
        
        // Testing the 2 room type
        // Change Nth() to 1 for king, 2 for king w/ balcony, 3 for suite
        await Page.Locator("button").Nth(2).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*CustomerInformation")); 
        
        // Fill in customer information 
        await Page.FillAsync("input[name='Input.FirstName']", "First");
        await Page.FillAsync("input[name='Input.LastName']", "Last"); 
        await Page.FillAsync("input[name='Input.Email']", "email@email.com");
        await Page.FillAsync("input[name='Input.CreditCardNumber']", "1234 5678 9123 4567"); 
        await Page.FillAsync("input[name='Input.CreditCardExpiration']", "12/34");
        await Page.FillAsync("input[name='Input.CreditCardCvv']", "123"); 
        
        // Submit customer information
        await Page.Locator("button").Nth(1).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*Confirmation"));
        
    } 
}