namespace Bliblioteca.Core.Domain.Entity
{
    public class Libro
    {
        public int Id {get; set;}
        public required string Titulo {get; set;}
        public  required string Autor {get; set;}  
        public required int AñoPublicacion {get; set;}
        public required string Genero {get; set;} 
        public required string Descripcion {get; set;} 
        public string? UsuarioId {get; set;}
    }
}
