using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class UserSignInForm
    {

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Email", Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
