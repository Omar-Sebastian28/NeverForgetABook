namespace Bliblioteca.Core.Aplication.Dto
{
    public class LibroDto
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public required int AñoPublicacion { get; set; }
        public required string Genero { get; set; }
        public required string Descripcion { get; set; }
        public string? UsuarioId { get; set; }
    }
}
