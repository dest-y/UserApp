using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
