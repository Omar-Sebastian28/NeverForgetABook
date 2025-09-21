using Biblioteca.Infraestructura.Identity.Entities;
using Bliblioteca.Core.Aplication.Dto.User;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Biblioteca.Infraestructura.Identity.Servicio
{
    public class AccountServicesForWebApi : BaseAccountServices, IAccountServicesForWebApi
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly JwtSettings _JwtSettings;

        public AccountServicesForWebApi(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, IEmailServices emailServices, IOptions<JwtSettings> JwtSettings) : base(userManager, emailServices)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _JwtSettings = JwtSettings.Value;
        }

        public virtual async Task<LoginReponseForApiDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginReponseForApiDto response = new()
            {   Nombre = "",
                Apellido = "",
                HasError = false,
            };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
            {
                response.HasError = true;
                response.Error = $"No se encontró ninguna cuenta asociada al nombre de usuario ingresado. Verifica que esté escrito correctamente o intenta con tu correo electrónico. Si el problema persiste, puedes crear una nueva cuenta o contactar al soporte.";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Tu cuenta aún no ha sido activada. Por favor, revisa tu correo electrónico y sigue el enlace de confirmación para completar el registro..";
                return response;
            }

            var result = await _singInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, true);

            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Error = "Has excedido el número máximo de intentos permitidos. Por seguridad, tu acceso ha sido bloqueado temporalmente. Podrás intentarlo nuevamente en 10 minutos.";
                }
                else
                {
                    response.Error = "Las credenciales ingresadas no son váli1das. Verifica tu correo y contraseña, y vuelve a intentarlo.";
                }
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

            response.Nombre = user.Nombre;
            response.Apellido = user.Apellido;  
            response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return response;
        }


        private async Task<JwtSecurityToken> GenerateJwtToken(AppUser user) 
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(role => new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id)
            }.Union(roleClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_JwtSettings.SecretKey));
            var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken        
            (
                issuer: _JwtSettings.Issuer,
                audience: _JwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_JwtSettings.DurationInMinutes),
                signingCredentials : singingCredentials
            );
            return token;
        }
    }
}
