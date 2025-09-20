using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.User;
using Bliblioteca.Core.Aplication.Helper;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API_Biblioteca.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountServicesForWebApi _accountServicesForWebApi;
        public AccountController(IAccountServicesForWebApi accountServicesForWebApi)
        {
            _accountServicesForWebApi = accountServicesForWebApi;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var result = await _accountServicesForWebApi.AuthenticateAsync(loginDto);
                if (!result.HasError)
                {
                    return Ok(result);
                }
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm] BasicUserDto basicUseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var responseUser = await _accountServicesForWebApi.RegisterAsync(new () 
                {
                    Nombre = basicUseDto.Nombre,
                    Apellido = basicUseDto.Apellido,
                    UserName = basicUseDto.UserName,
                    Phone = basicUseDto.Phone,
                    Email = basicUseDto.Email,
                    Password = basicUseDto.Password,
                    Role = Roles.Usuario.ToString(),
                });
                if (!responseUser.HasError || responseUser.Error is not null)
                {
                    var imagenFile = UploadFile.Upload(basicUseDto.ImagenPerfil, responseUser.Id, "FotoPerfilUser");
                    var editResult = await _accountServicesForWebApi.EditUser(new () 
                    {
                        UserId = responseUser.Id,
                        Nombre = responseUser.Nombre,
                        Apellido = responseUser.Apellido,
                        Email = responseUser.Email,
                        Password = responseUser.Password,
                        UserName = responseUser.UserName,
                        Phone = responseUser.Phone,
                        Role = basicUseDto.Role,
                        ImagenPerfil = imagenFile
                    }, true);
                    return Ok(responseUser);                   
                }
                return BadRequest(responseUser.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
