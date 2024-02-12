using Lafatkotob.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("User ID or token is missing.");
        }

        token = System.Net.WebUtility.UrlDecode(token);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            // For debug purposes, consider logging the error details to understand why it fails
            return BadRequest($"Email confirmation failed. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return RedirectToAction("Index", "Home");
    }
}
