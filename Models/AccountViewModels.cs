using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}