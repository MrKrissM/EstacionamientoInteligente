// Models/Usuario.cs
using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}