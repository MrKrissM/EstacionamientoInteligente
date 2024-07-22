using System.ComponentModel.DataAnnotations;

namespace EstacionamientoInteligente.Models
{
   
 public class LoginViewModel
{
    [Required]
    [Display(Name = "Nombre de usuario")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}
}