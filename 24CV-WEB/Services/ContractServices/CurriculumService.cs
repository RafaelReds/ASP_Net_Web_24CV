using _24CV_WEB.Models;
using _24CV_WEB.Repository;
using _24CV_WEB.Repository.CurriculumDAO;
using _24CV_WEB.Services.Contracts;

namespace _24CV_WEB.Services.ContractServices
{
	public class CurriculumService : ICurriculumService
	{
		private CurriculumRepository _repository;

        public CurriculumService(ApplicationDbContext context)
        {
            _repository = new CurriculumRepository(context);
        }

        public async Task<ResponseHelper> Create(Curriculum model)
		{
			ResponseHelper responseHelper = new ResponseHelper();

			try
			{
				string filePath = "";
				string fileName = "";

				if (model.Foto != null && model.Foto.Length > 0)
				{
					 fileName = Path.GetFileName(model.Foto.FileName);
				     filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Fotos",fileName);
				}

				model.RutaFoto = fileName;

				//Copia el archivo en un directorio.
				using (var fileStream = new FileStream(filePath,FileMode.Create))
				{
					await model.Foto.CopyToAsync(fileStream);
				}

				if (_repository.Create(model) > 0)
				{
					responseHelper.Success = true;
					responseHelper.Message = $"Se agregó el curriculum de {model.Nombre} con éxito.";
				}
				else
				{
					responseHelper.Message = "Ocurrió un error al agregar el dato.";
				}
			}
			catch (Exception e)
			{
				responseHelper.Message = $"Ocurrió un error al agregar el dato. Error: {e.Message}";
			}


			return responseHelper;	
		}

		public ResponseHelper Delete(int id)
		{
			throw new NotImplementedException();
		}

		public List<Curriculum> GetAll()
		{
			List<Curriculum> list = new List<Curriculum>();

			try
			{
				list = _repository.GetAll();
			}
			catch (Exception e)
			{
				throw;
			}

			return list;



		}

		 public Curriculum GetById(int id)
        {
            try
            {
                var curriculum = _repository.GetById(id);
                return curriculum;
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al obtener el currículum. Error: {e.Message}");
            }
        }

        public async Task<ResponseHelper> Update(Curriculum model)
        {
            ResponseHelper responseHelper = new ResponseHelper();

            try
            {
                // Verifica si el currículum con el ID especificado existe
                var existingCurriculum = _repository.GetById(model.Id);
                if (existingCurriculum == null)
                {
                    responseHelper.Message = "Currículum no encontrado.";
                    return responseHelper;
                }

                // Copia los campos editables desde el modelo recibido
                existingCurriculum.Nombre = model.Nombre;
                existingCurriculum.Apellidos = model.Apellidos;
                existingCurriculum.CURP = model.CURP;
                existingCurriculum.FechaNacimiento = model.FechaNacimiento;
                existingCurriculum.Dirección = model.Dirección;
                existingCurriculum.Email = model.Email;
                existingCurriculum.PorcentajeIngles = model.PorcentajeIngles;

                // Actualiza la ruta de la foto si se proporciona una nueva foto
                if (model.Foto != null && model.Foto.Length > 0)
                {
                    string fileName = Path.GetFileName(model.Foto.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Fotos", fileName);
                    existingCurriculum.RutaFoto = fileName;

                    // Copia el archivo en el directorio
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Foto.CopyToAsync(fileStream);
                    }
                }

                // Guarda los cambios en el repositorio
                if (_repository.Update(existingCurriculum) > 0)
                {
                    responseHelper.Success = true;
                    responseHelper.Message = $"Curriculum de {model.Nombre} actualizado con éxito.";
                }
                else
                {
                    responseHelper.Message = "Error al actualizar el currículum.";
                }
            }
            catch (Exception e)
            {
                responseHelper.Message = $"Error al actualizar el currículum. Error: {e.Message}";
            }

            return responseHelper;
        }
    }
}
