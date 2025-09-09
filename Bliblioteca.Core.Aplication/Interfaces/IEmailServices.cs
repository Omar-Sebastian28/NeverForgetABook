using Bliblioteca.Core.Aplication.Dto.Email;

namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface IEmailServices
    {
        Task SendAsync(EmailRequestDto dto);
    }
}