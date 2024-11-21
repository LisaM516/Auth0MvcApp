using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
         string userName = User.Identity.IsAuthenticated? User.Claims.FirstOrDefault(c => c.Type == "name")?.Value: "Guest";

        // Pass the username to the view
        ViewBag.UserName = userName;
        return View();
    }

    [Authorize]
    public IActionResult Profile()
    {
        string userName = User.Identity.IsAuthenticated? User.Claims.FirstOrDefault(c => c.Type == "name")?.Value: "Guest";
        ViewBag.Username = userName;
        foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

        return View();
    }

    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Auth0");
    }

    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                       "Auth0", CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
