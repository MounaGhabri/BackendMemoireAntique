using Microsoft.AspNetCore.Identity;

namespace Projet.Models
{
    public class User : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}