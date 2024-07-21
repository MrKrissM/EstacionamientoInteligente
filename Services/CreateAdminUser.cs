using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Identity;

public class CreateAdminUser : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreateAdminUser> _logger;

    public CreateAdminUser(IServiceProvider serviceProvider, ILogger<CreateAdminUser> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminEmail = "cristianmbetancur@gmail.com";
            var adminPassword = "Parking003.";

            _logger.LogInformation("Iniciando creación de usuario administrador");

            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser == null)
            {
                _logger.LogInformation("Usuario administrador no existe, creando...");
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    AdminEmail = adminEmail,  // Asegúrate de establecer este valor
                    // AdminPassword = adminPassword  // Agrega esta línea si necesitas esta propiedad

                };

                // Aquí es donde agregas el logging adicional
                _logger.LogInformation("Intentando crear usuario con Email: {Email}", adminEmail);
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                _logger.LogInformation("Resultado de la creación del usuario: {Result}", result.Succeeded);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuario administrador creado exitosamente");
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        _logger.LogInformation("Rol Admin no existe, creando...");
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("Rol Admin asignado al usuario administrador");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Error al crear usuario administrador: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            else
            {
                _logger.LogInformation("Usuario administrador ya existe");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el usuario administrador");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}