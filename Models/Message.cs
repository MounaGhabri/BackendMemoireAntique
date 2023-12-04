using System.ComponentModel.DataAnnotations;

namespace Projet.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string MessageContent { get; set; }
    }

}
