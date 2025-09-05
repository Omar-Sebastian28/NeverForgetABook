using API_Biblioteca.Controllers;
using Asp.Versioning;
using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Biblioteca.v1
{
    [ApiVersion("1.0")]
    public class LibroController : BaseApiController
    {
        private readonly ILibroServices _libroServices;

        public LibroController(ILibroServices libroServices)
        {
            _libroServices = libroServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listLibros = await _libroServices.GetAllListAsync();

                if (listLibros is null || listLibros.Count == 0) { return NoContent(); }
                return Ok(listLibros);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
