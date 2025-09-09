using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Infraestructura.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public required string Nombre { get; set; }

        public required string Apellido { get; set; }

        public string? ImagenPerfil { get; set; }
    }
}
