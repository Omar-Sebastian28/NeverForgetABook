using API_Biblioteca.Controllers;
using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.Libro;
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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] LibroDto Dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _libroServices.AddAsync(Dto);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "La creacion fallo.");
                }
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibroDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _libroServices.GetByIdAsync(id);
                if (!result.exito || result.dtoEntity is null)
                {
                    return NoContent();
                }
                return Ok(result.dtoEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] LibroDto dto ,int id)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            try
            {
                var result = await _libroServices.UpdateAsync(dto,id);
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
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _libroServices.DeleteAsync(id);
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
