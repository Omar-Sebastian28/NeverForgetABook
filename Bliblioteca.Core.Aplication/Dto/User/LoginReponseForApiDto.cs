namespace Bliblioteca.Core.Aplication.Dto.User
{
    public class LoginReponseForApiDto
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Error { get; set; }
        public bool HasError { get; set; }
        public List<string>? Roles { get; set; }

        public string? AccessToken { get; set; }
    }
}
