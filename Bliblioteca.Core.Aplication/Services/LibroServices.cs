using AutoMapper;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;

namespace Bliblioteca.Core.Aplication.Services
{
    public class LibroServices : GenericServices<Libros,LibroDto>, ILibroServices
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _autoMapper;

        public LibroServices(ILibroRepository libroRepository, IMapper autoMapper) : base(libroRepository, autoMapper)
        {
            _libroRepository = libroRepository;
            _autoMapper = autoMapper;
        }     

        public IQueryable<LibroDto> GetQuery()
        {
            var query = _libroRepository.GetQuery();
            try
            {
                var queryDto = _autoMapper.ProjectTo<LibroDto>(query);
                return queryDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mapear la consulta a DTO: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
                return Enumerable.Empty<LibroDto>().AsQueryable();
        }   }
    }
}
