using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels.AccountVM
{
    public class RegisterVM
    {
        [Required, MaxLength(64)]
        public string Username { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }
    }
}
