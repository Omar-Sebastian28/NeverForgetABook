using Bliblioteca.Core.Domain.Enums;

namespace Bliblioteca.Core.Aplication.Dto.User
{
    public class EditUserDto
    {
        public required string UserId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }

        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string ImagenPerfil { get; set; }
        public required Roles Rol { get; set; }
    }
}
