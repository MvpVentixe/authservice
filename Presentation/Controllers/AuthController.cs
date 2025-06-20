using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, SignInManager<AppUser> signInManager, IConfiguration config) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly IConfiguration _config = config;


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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _authService.GetUserByEmailAsync(form.Email);
                var token = _authService.GenerateJwtToken(user!, _config);
                return Ok(new { token, message = "Login successful" });
            }

            return Unauthorized(new { success = false, message = "Invalid login attempt." });
        }

        [HttpPost("signout")]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Signed out successfully" });
        }

        [HttpGet("booking/{userId}")]
        public async Task<ActionResult<BookingDto>> GetBookingInfo(string userId)
        {
            try
            {
                var bookingInfo = await _authService.GetAuthInfoToBookingAsync(userId);
                return Ok(bookingInfo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
}