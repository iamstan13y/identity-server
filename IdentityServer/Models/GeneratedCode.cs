using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class GeneratedCode
    {
        [Key]
        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public string? Code { get; set; }
        public DateTime DateCreated { get; set; }
    }
}