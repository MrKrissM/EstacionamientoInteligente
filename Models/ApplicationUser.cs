using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        public string? AdminEmail { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();

        [Display(Name = "Nombre de visualización")]
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                    return $"{FirstName} {LastName}";
                else if (!string.IsNullOrEmpty(FirstName))
                    return FirstName;
                else if (!string.IsNullOrEmpty(LastName))
                    return LastName;
                else
                    return UserName;
            }
        }
    }
}