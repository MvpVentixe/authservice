using Presentation.Models;

namespace Presentation.Services
{
    public interface IAuthService
    {
        Task<BookingDto> GetAuthInfoToBookingAsync(string id);
        Task<int> CreateAsync(UserSignUpForm form);
        string GenerateJwtToken(AppUser user, IConfiguration config);
        Task<AppUser?> GetUserByEmailAsync(string email);
    }
}