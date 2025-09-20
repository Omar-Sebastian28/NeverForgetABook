using Bliblioteca.Core.Aplication.Dto.Response;
using Bliblioteca.Core.Aplication.Dto.User;

namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface IBaseAccountServices
    {
        Task<DtoUser?> BuscarUsuarioPorEmail(string email);
        Task<DtoUser?> BuscarUsuarioPorId(string Id);
        Task<DtoUser?> BuscarUsuarioPorUserName(string userName);
        Task<ConfirmRequestDto> ConfirmAccount(string userId, string token);
        Task<ResetPasswordResponseDto> ConfirmForgotPassword(ResetPasswordRequestDto dto);
        Task<DeleteResponseDto> DeleteAsync(string userId);
        Task<ResponseDto> EditUser(EditUserDto saveUserDto, bool? creando = false);
        Task<ResetPasswordResponseDto> ForgotPassword(ResetPasswordResponseDto dto);
        Task<List<DtoUser>> GetAllUser(bool? isActive = true);
        Task<ResponseDto> RegisterAsync(CreateUserDto saveUserDto);
    }
}