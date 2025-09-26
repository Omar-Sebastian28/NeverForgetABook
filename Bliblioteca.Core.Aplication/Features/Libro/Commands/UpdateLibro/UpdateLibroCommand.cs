using AutoMapper;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;
using MediatR;

namespace Bliblioteca.Core.Aplication.Features.Libro.Commands.UpdateLibro
{
    public class UpdateLibroCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public required int AñoPublicacion { get; set; }
        public required string Genero { get; set; }
        public required string Descripcion { get; set; }
        public string? UsuarioId { get; set; }
    }

    public class UpdateLibroCommandHandler : IRequestHandler<UpdateLibroCommand, bool>
    {
        private readonly ILibroRepository _libroRepositorio;
        private readonly IMapper _mapper;   

        public UpdateLibroCommandHandler(ILibroRepository libroRepositorio, IMapper mapper)
        {
            _libroRepositorio = libroRepositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateLibroCommand command, CancellationToken cancellationToken)
        {
            var getEntity = await _libroRepositorio.GetByIdAsync(command.Id);
            if (getEntity.entityDb is null)
            {
                throw new ArgumentException("El libro no existe.");
            }
            try
            {
                var entity = _mapper.Map<Libros>(command);
                await _libroRepositorio.UpdateAsync(entity, entity.Id);
                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error al actualizar el libro: {ex.Message}", ex);
            }
    }   }
}
