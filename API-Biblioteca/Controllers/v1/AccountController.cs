using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.User;
using Bliblioteca.Core.Aplication.Helper;
using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace API_Biblioteca.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de cuentas: Crud")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountServicesForWebApi _accountServicesForWebApi;
        public AccountController(IAccountServicesForWebApi accountServicesForWebApi)
        {
            _accountServicesForWebApi = accountServicesForWebApi;
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(

            OperationId = "Authenticate",
            Summary = "Autentica un usuario y genera un token JWT",
            Description = "Permite autenticar un usuario con sus credenciales y, si son válidas, genera un token JWT para acceder a recursos protegidos."

        )]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            
            OperationId = "Register",
            Summary = "Registra un nuevo usuario",
            Description = "Permite registrar un nuevo usuario en el sistema con los datos proporcionados."
        )]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
         
            OperationId = "ConfirmAccount",
            Summary = "Confirma la cuenta de un usuario",
            Description = "Permite confirmar la cuenta de un usuario utilizando un token enviado por correo electrónico."
        )]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        
            OperationId = "ForgotPassword",
            Summary = "Genera un token para restablecer la contraseña",
            Description = "Permite genera un token para restablecer la contraseña y lo envía al correo electrónico del usuario."
        )]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            
            OperationId = "ConfirmChangePassword",
            Summary = "Confirma el cambio de contraseña",
            Description = "Permite confirmar el cambio de contraseña utilizando el token enviado al correo electrónico del usuario."
        )]
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
