using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static UserApp.Services.UserService.UserService;

namespace UserApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [UniqEmail]
        public string Email { get; set; }

        [Required]
        [Range(1, 150, ErrorMessage = "Only numbers in range 1-150 allowed")]
        public int Age { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
