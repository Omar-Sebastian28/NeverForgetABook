using AutoMapper;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Bliblioteca.Core.Aplication.Features.Libro.Commands.CreateLibro
{
    /// <summary>
    /// Parametros necesarios para crear un nuevo libro.
    /// </summary>
    public class CreateLibroCommand : IRequest<bool>
    {
        ///<Example>La bella y la bestia</Example>
        [SwaggerParameter(Description ="Titulo del libro.")]
        public required string Titulo { get; set; }

        ///<Example>Gabriel García Márquez</Example>
        [SwaggerParameter(Description = "Autor del libro.")]
        public required string Autor { get; set; }

        [SwaggerParameter(Description = "año de publicación del libro.")]
        public required int AñoPublicacion { get; set; }

        ///<Example>Masculino/Femenino</Example>
        public required string Genero { get; set; }

        [SwaggerParameter(Description = "Una breve descripcion sobre el libro que se quiere registrar.")]
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
