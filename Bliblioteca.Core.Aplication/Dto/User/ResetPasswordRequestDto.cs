namespace Bliblioteca.Core.Aplication.Dto.User
{
    public class ResetPasswordRequestDto
    {
        public required string UserName { get; set; }

        public required string Token { get; set; }

        public required string Password { get; set; }
    }
}
