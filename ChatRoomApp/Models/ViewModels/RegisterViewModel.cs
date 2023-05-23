using System.ComponentModel.DataAnnotations;

namespace ChatRoomApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User Name cannot be empty.")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be empty.")]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Comfirm Password Name cannot be empty.")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match")]
        [MaxLength(20)]
        public string ComfirmPassword { get; set; }
    }
}
