using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EstacionamientoInteligente.Models;
using EstacionamientoInteligente.Data;
using Microsoft.EntityFrameworkCore;

namespace EstacionamientoInteligente.Controllers
{
    public class AccountController : Controller
    {
        private readonly EstacionamientoContext _context;

        public AccountController(EstacionamientoContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "El nombre de usuario ya existe");
                    return View(model);
                }

                var usuario = new Usuario
                {
                    Username = model.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Usuario o contraseña inválidos");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}