using System.ComponentModel.DataAnnotations;

namespace Account.Api.DTO
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }

        public string Puesto { get; set; }
    }
}