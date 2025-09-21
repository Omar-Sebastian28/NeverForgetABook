using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.User;
using Bliblioteca.Core.Aplication.Helper;
using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_Biblioteca.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        private readonly IAccountServicesForWebApi _accountServicesForWebApi;

        public UserController(IAccountServicesForWebApi accountServicesForWebApi)
        {
            _accountServicesForWebApi = accountServicesForWebApi;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DtoUser>))]
        public async Task<IActionResult> GetAllUser() 
        {
            try
            {
                var listUser = await _accountServicesForWebApi.GetAllUser(true);
                if (listUser.Count != 0) 
                {
                    return NoContent();
                }
                return Ok(listUser);
            }
            catch (Exception ex) 
            {
               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditUser([FromForm] BasicUserDto basicUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Debe completar los campos que son obligatorios." });
            }
            try
            {
                var userSearch = await _accountServicesForWebApi.BuscarUsuarioPorUserName(basicUserDto.UserName);

                var userUpdate = await _accountServicesForWebApi.EditUser(new EditUserDto
                {
                    UserId = userSearch?.Id,
                    Nombre = basicUserDto.Nombre,
                    Apellido = basicUserDto.Apellido,
                    UserName = basicUserDto.UserName,
                    Email = basicUserDto.Email,
                    Password = basicUserDto.Password ?? string.Empty,
                    ImagenPerfil = UploadFile.Upload(basicUserDto.ImagenPerfil, userSearch?.Id, "FotoEditadaUser"),
                    Phone = basicUserDto.Phone,
                    Rol = basicUserDto.Rol
                });

                if (!userUpdate.HasError && userUpdate.Error != null)
                {
                    return Accepted(new { Message = "Usuario ha sido actualizado con exito." });
                }

                return BadRequest(new { Message = userUpdate.Error });
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }        
        }
    }
}
