using Presentation.Models;

namespace Presentation.Services
{
    public interface IAuthService
    {
        Task<int> CreateAsync(UserSignUpForm form);
    }
}