using Microsoft.AspNetCore.Identity;

namespace EstacionamientoInteligente.Models

{
    public class ApplicationUser : IdentityUser
    {
        public string? AdminEmail { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();

    }


}