using Asp.Versioning;
using Bliblioteca.Core.Aplication.Dto.User;
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
    }
}
