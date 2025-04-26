using System.ComponentModel.DataAnnotations;

namespace PresentationMVCWebApp.Models;

public class SignInViewModel
{
    /* [RegularExpression(@"")] */
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    public string Email { get; set; } = null!;

    /* [RegularExpression(@"")] */
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Required")]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; }
}
