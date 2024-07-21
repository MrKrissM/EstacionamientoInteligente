using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
}

public class CreateUserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; }

    [EmailAddress]
    public string? AdminEmail { get; set; }
}

public class EditPermissionsViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<string> Permissions { get; set; }
}

public class EditRolesViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
    public IList<string> UserRoles { get; set; }
    public List<string> SelectedRoles { get; set; }
}

public class DeleteUserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}
