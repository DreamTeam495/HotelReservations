using System.Text.RegularExpressions; 
using Microsoft.Playwright; 
using Microsoft.Playwright.NUnit; 
 
namespace tests; 
 
[Parallelizable(ParallelScope.Self)] 
[TestFixture] 
public class LoginTest : PageTest 
{ 
    [Test] 
    public async Task LoginAndCheckReportPageViewableAndLogOut() 
    { 
        await Page.GotoAsync("https://localhost:7022"); 
         
        await Page.ClickAsync("text=Login"); 
 
        // Fill log in information 
        await Page.FillAsync("input[name='Input.Email']", "admin@dreamhotel.com"); 
         
        await Page.FillAsync("input[name='Input.Password']", "Password1!");

        // Log in
        await Page.Locator("button").Nth(1).ClickAsync();
        
        // create a locator 
        var report = Page.GetByRole(AriaRole.Link, new() { Name = "Report" }); 
 
        // Expect the report tag to be shown as we're logged in as admin
        await Expect(report).ToHaveAttributeAsync("href", "/Rooms/Report");

        // Log out
        await Page.Locator("button").Nth(1).ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*Logout"));
    } 
}