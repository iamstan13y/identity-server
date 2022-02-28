using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateTime DateCreated { get; set; }
        [NotMapped]
        public string? Token { get; set; }

    }
}
