using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

[Route("api/login")]
public class LoginController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        // var user = await _userManager.FindByEmailAsync(email);
        // if (user is null || !await _signInManager.CheckPasswordSignInAsync(user, password, false))
        // {
        //     TempData["Error"] = "Invalid email or password";
        //     return View();
        // }
        //
        // await _signInManager.SignInAsync(user, isPersistent: true);
        // return Redirect("/connect/authorize"); 
        return View(); // Todo
    }
}
