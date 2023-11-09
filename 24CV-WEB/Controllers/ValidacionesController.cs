using _24CV_WEB.Models;
using _24CV_WEB.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace _24CV_WEB.Controllers
{
    public class ValidacionesController : Controller
    {
        private readonly ICurriculumService _curriculumService;

        public ValidacionesController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult CurriculumVitae(int id) //Controlador para función CRUD de Ver curriculum
		{
            //return View();

            var curriculum = _curriculumService.GetById(id);
			if (curriculum != null)
			{
				return View(curriculum);
			}
			else
			{
				return NotFound();
			}

		}


		[HttpPost]
        public IActionResult EnviarInformacion(Curriculum model) {

            string mensaje = "";
            //model.RutaFoto = "FakePath";

            if (ModelState.IsValid)
            {
                var response = _curriculumService.Create(model).Result;

                mensaje = response.Message;
                TempData["msj"] = mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                mensaje = "Datos incorrectos";
                TempData["msj"] = mensaje;

                return View("Index",model);
            }

        }


        public IActionResult Lista()
        {
            return View(_curriculumService.GetAll());
        }

        //FUNCION EDITAR MODIFICAR CURRICULUM (La vista se llama "Editar".cshtml)
        public IActionResult Editar(int id)
        {
            var curriculum = _curriculumService.GetById(id);
            if (curriculum != null)
            {
                return View(curriculum);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditarCV(Curriculum model)
        {
			if (ModelState.IsValid)
			{
				var response = await _curriculumService.Update(model); // Espera la tarea

				if (response.Success)
				{
					TempData["msj"] = $"Curriculum de {model.Nombre} actualizado con éxito.";
					return RedirectToAction("Lista");
				}
				else
				{
					TempData["msj"] = "Error al actualizar el currículum.";
					return View(model);
				}
			}
			else
			{
				TempData["msj"] = "Datos incorrectos";
				return View(model);
			}
		}

    }
}
