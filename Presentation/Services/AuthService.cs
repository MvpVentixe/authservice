using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Services
{
    public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IAuthService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager; //can use to sign in directly


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
    }
}
