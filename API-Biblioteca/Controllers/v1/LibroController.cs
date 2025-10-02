using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Aplication.Features.Libro.Commands.CreateLibro;
using Bliblioteca.Core.Aplication.Features.Libro.Commands.DeleteLibro;
using Bliblioteca.Core.Aplication.Features.Libro.Commands.UpdateLibro;
using Bliblioteca.Core.Aplication.Features.Libro.Queries.GetByIdLibro;
using Bliblioteca.Core.Aplication.Features.Libro.Queries.GetQueryLibro;
using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace API_Biblioteca.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimiento de los libros: Crud")]
    public class LibroController : BaseApiController
    {
        private readonly ILibroServices _libroServices;

        public LibroController(ILibroServices libroServices)
        {
            _libroServices = libroServices;
        }


        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibroDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(

            OperationId = "GetAllLibros",
            Summary = "Obtiene todos los libros",
            Description = "Obtiene todos los libros registrados sin filtros."
          //  Tags = new[] { "LibroEndpoints" }
        )]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listLibros = await Mediator.Send(new GetAllLibroQuery());
                if (listLibros is null || listLibros.Count == 0)
                {
                    return NoContent();
                }
                return Ok(listLibros);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(

            OperationId = "CreateLibro",
            Summary = "Crea un nuevo libro",
            Description = "Crea un nuevo libro en el sistema con los datos proporcionados."
          //  Tags = new[] { "LibroEndpoints" }
         )]
        public async Task<IActionResult> Create([FromBody] CreateLibroCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await Mediator.Send(command);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "La creacion fallo.");
                }
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibroDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            
            OperationId = "GetLibroById",
            Summary =  "Obtiene un libro por su ID",
            Description = "Obtiene un libro registrado en el sistema mediante su identificador único (ID)."
          //  Tags = new[] { "LibroEndpoints" }
        )]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await Mediator.Send(new GetByIdLibroQuery() { Id = id });
                if (result is null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(

            OperationId = "UpdateLibro",
            Summary = "Actualiza un libro existente",
            Description = "Actualiza los datos de un libro existente en el sistema."
          //  Tags = new[] { "LibroEndpoints" }
        )]
        public async Task<IActionResult> Update([FromBody] UpdateLibroCommand command, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Complete los campos requeridos.");
            }

            if (command.Id == id)
            {
                return BadRequest("Los ID no coincden ");
            }
            try
            {
                var result = await Mediator.Send(command);
                if (!result)
                {
                    return NoContent();
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(

            OperationId = "DeleteLibro",
            Summary = "Elimina un libro existente",
            Description = "Elimina un libro existente en el sistema mediante su identificador único (ID)."
           // Tags = new[] { "LibroEndpoints" }
        )]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteLibroCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Complete los campos requeridos.");
            }
            if (command.Id != id)
            {
                return BadRequest("Los ID no coincden ");
            }
            try
            {
                var result = await Mediator.Send(command);
                if (!result)
                {
                    return NoContent();
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
