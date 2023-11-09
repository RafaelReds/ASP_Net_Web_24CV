using _24CV_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace _24CV_WEB.Controllers
{
    public class EjemplosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contacto()
        {
            Persona persona = new Persona();
            persona.Nombre = "Rafael";
            persona.Apellidos = "Rojas";
            persona.FechaNacimiento = new DateTime(2004, 07, 18);

            return View(persona);
        }
    }
}
