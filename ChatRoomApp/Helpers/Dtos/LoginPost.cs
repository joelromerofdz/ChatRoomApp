using System.ComponentModel.DataAnnotations;

namespace ChatRoomApp.Helpers.Dtos
{
    public class LoginPost
    {
        [Required(ErrorMessage = "User Name cannot be empty.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
