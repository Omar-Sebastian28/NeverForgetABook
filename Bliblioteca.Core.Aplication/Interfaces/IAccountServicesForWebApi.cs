using Bliblioteca.Core.Aplication.Dto.User;

namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface IAccountServicesForWebApi : IBaseAccountServices
    {
        Task<LoginReponseForApiDto> AuthenticateAsync(LoginDto loginDto);
    }
}
