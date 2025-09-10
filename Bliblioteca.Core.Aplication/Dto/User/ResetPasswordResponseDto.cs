namespace Bliblioteca.Core.Aplication.Dto.User
{
    public class ResetPasswordResponseDto
    {
        public bool HasError { get; set; }
        public string? Origin { get; set; }

        public string? Error { get; set; }

        public required string UserName { get; set; }
    }
}
