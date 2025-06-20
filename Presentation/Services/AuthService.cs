using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Services
{
    public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config) : IAuthService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager; //can use to sign in directly
        private readonly IConfiguration _config = config;

        //funktioner 

        public async Task<int> CreateAsync(UserSignUpForm form)
        {
            if (form == null) return 400;

            if (await _userManager.Users.AnyAsync(e => e.Email == form.Email)) return 409;

            var appUser = new AppUser
            {
                UserName = form.Email,
                Email = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName
            };

            var result = await _userManager.CreateAsync(appUser, form.Password);
            if (result.Succeeded) return 201;
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"❌ Identity error: {error.Code} - {error.Description}");
            }
            return 500;
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<BookingDto> GetAuthInfoToBookingAsync(string id) 
        {
            return await _userManager.Users.Where(u => u.Id == id).Select(u => new BookingDto 
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
            }).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("User not found");
        }


        public string GenerateJwtToken(AppUser user, IConfiguration config)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
