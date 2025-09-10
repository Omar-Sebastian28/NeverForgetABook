using AutoMapper;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;

namespace Bliblioteca.Core.Aplication.Services
{
    public class LibroServices : GenericServices<Libro,LibroDto>, ILibroServices
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _autoMapper;

        public LibroServices(ILibroRepository libroRepository, IMapper autoMapper) : base(libroRepository, autoMapper)
        {
            _libroRepository = libroRepository;
            _autoMapper = autoMapper;
        }     
    }
}
