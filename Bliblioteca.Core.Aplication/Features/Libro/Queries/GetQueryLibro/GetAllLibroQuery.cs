using AutoMapper;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Domain.Interfaces;
using MediatR;

namespace Bliblioteca.Core.Aplication.Features.Libro.Queries.GetQueryLibro
{
    public class GetAllLibroQuery : IRequest<IList<LibroDto>>
    {
    }

    public class GetAllLibroQueryHandler : IRequestHandler<GetAllLibroQuery, IList<LibroDto>>
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _mapper;   

        public GetAllLibroQueryHandler(ILibroRepository libroRepository, IMapper mapper)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
        }
        public  async Task<IList<LibroDto>> Handle(GetAllLibroQuery query, CancellationToken cancellationToken)
        {
            var listEntity = await _libroRepository.GetAllListAsync();
            if (listEntity is null) 
            {
                throw new ArgumentException("No hay libros disponibles en stock.");
            }
            try
            {
                var listDto = _mapper.Map<List<LibroDto>>(listEntity);
                return listDto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los libros", ex);
            }
        }
    }
}
