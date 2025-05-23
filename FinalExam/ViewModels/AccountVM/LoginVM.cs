using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels.AccountVM
{
    public class LoginVM
    {
        [MaxLength(128)]
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
    }
}