using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EstacionamientoInteligente.Models;
using Microsoft.AspNetCore.Authorization;

namespace EstacionamientoInteligente.Controllers;

[Authorize] // Esto aplicará la autorización a todas las acciones en este controlador
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}