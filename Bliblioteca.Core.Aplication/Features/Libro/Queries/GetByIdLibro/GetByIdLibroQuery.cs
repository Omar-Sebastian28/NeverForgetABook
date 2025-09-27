using AutoMapper;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Domain.Interfaces;
using MediatR;

namespace Bliblioteca.Core.Aplication.Features.Libro.Queries.GetByIdLibro
{
    public class GetByIdLibroQuery : IRequest<LibroDto>
    {
        public required int Id { get; set; }
    }

    public class GetByIdLibroQueryHandler : IRequestHandler<GetByIdLibroQuery, LibroDto>
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _mapper;

        public GetByIdLibroQueryHandler(ILibroRepository libroRepository, IMapper mapper)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
        }

        public async Task<LibroDto> Handle(GetByIdLibroQuery query, CancellationToken cancellationToken)
        {
            var entity = await _libroRepository.GetByIdAsync(query.Id);
            if (entity.entityDb is null || !entity.exito)
            {
                throw new ArgumentException("El libro que buscas no existe.");
            }
            try
            {
                var dto = _mapper.Map<LibroDto>(entity.entityDb);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al mapear la entidad a DTO", ex);
            }
        }
    }
}
