using AutoMapper;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;
using MediatR;

namespace Bliblioteca.Core.Aplication.Features.Libro.Commands.CreateLibro
{
    public class CreateLibroCommand : IRequest<bool>
    {
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public required int AñoPublicacion { get; set; }
        public required string Genero { get; set; }
        public required string Descripcion { get; set; }
        public string? UsuarioId { get; set; }
    }


    public class CreateLibroCommandHandler : IRequestHandler<CreateLibroCommand, bool>
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMapper _mapper;

        public CreateLibroCommandHandler(ILibroRepository libroRepository, IMapper mapper)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateLibroCommand command, CancellationToken cancellationToken) 
        {
            if (command is null)
            {
                return false;
            }
            try
            {   var entity = _mapper.Map<Libros>(command);
                await _libroRepository.AddAsync(entity);
                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error al crear el libro: {ex.Message}", ex);
            }
        }
    }
}
