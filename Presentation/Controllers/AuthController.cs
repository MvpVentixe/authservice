using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, SignInManager<AppUser> signInManager) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<AppUser> _signInManager = signInManager;


    [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpForm form) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.CreateAsync(form);

        return result switch
            {
                201 => StatusCode(201, new { success = true, message = "User created successfully." }),
                400 => BadRequest(new { success = false, message = "Invalid user data." }),
                409 => Conflict(new { success = false, message = "Email already in use." }),
                _ => StatusCode(500, new { success = false, message = "An error occurred while creating the user." })
            };

        }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] UserSignInForm form)
    {
        if (ModelState.IsValid) 
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
            if (result.Succeeded) return Ok(new { message = "Login Successful" });
        }

        return Unauthorized(new { message = "Invalid login attempt. Please check your email and password." });

    }
}