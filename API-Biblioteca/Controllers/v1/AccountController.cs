using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.User;
using Bliblioteca.Core.Aplication.Helper;
using Bliblioteca.Core.Aplication.Interfaces;
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

        [HttpPost("login")]
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

        [HttpPost("register")]
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
                    Role = basicUseDto.Rol.ToString(),
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
                        Rol = basicUseDto.Rol,
                        ImagenPerfil = imagenFile
                    }, true);
                    return Created();                   
                }
                return BadRequest(responseUser.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("confirm-account")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userName, string token)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(token)) 
            {
                return BadRequest("El nombre de usuario y el token son obligatorios.");
            }
            try
            {
                var result = await _accountServicesForWebApi.ConfirmAccount(userName, token);
                if (!result.HasError && result.Message is not null)
                {
                    return Accepted(result.Message);
                }

                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost("reset-password-token")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword([FromQuery] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("El nombre de usuario es obligatorio.");
            }
            try
            {
                var result = await _accountServicesForWebApi.ForgotPassword(new ResetPasswordResponseDto 
                {
                    UserName = userName                
                });

                if (!result.HasError && result.Error is null)
                {
                    return NoContent();
                }

                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("confirm-change-password")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmChangePassword([FromForm] ResetPasswordRequestDto changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("No ha completado la informacion requerida.");
            }
            try
            {
                var result = await _accountServicesForWebApi.ConfirmForgotPassword(changePassword);
                if (!result.HasError && result.Error is null)
                {
                    return Accepted(new { Message = result.Message }); 
                }
                return BadRequest(new { Error = result.Error });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
