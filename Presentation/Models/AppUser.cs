using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class AppUser: IdentityUser
    {
        [Required]
        [ProtectedPersonalData]
        public string FirstName { get; set; } = null!;
        [Required]
        [ProtectedPersonalData]
        public string LastName { get; set; } = null!;
    }
}
