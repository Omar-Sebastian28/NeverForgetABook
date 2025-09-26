using Bliblioteca.Core.Aplication.Dto.Libro;

namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface ILibroServices : IGenericServices<LibroDto>
    {
        IQueryable<LibroDto> GetQuery();
    }
}
