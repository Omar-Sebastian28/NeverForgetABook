using Bliblioteca.Core.Domain.Interfaces;
using MediatR;

namespace Bliblioteca.Core.Aplication.Features.Libro.Commands.DeleteLibro
{
    /// <summary>
    /// Parametro necesario para borrar un libro por (ID)
    /// </summary>
    public class DeleteLibroCommand : IRequest<bool>
    {
        ///<Example>7</Example>
        public int Id { get; set; }
    }

    public class DeleteLibroCommandHandler : IRequestHandler<DeleteLibroCommand, bool>
    {
        private readonly ILibroRepository _libroRepository;

        public DeleteLibroCommandHandler(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        public async Task<bool> Handle(DeleteLibroCommand command, CancellationToken cancellationToken)
        {
            var getEntity = await _libroRepository.GetByIdAsync(command.Id);
            if (getEntity.entityDb is null)
            {
                throw new ArgumentException("El libro no existe.");
            }
            try
            {
                await _libroRepository.DeleteAsync(command.Id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el libro: {ex.Message}");
            }
        }
    }
}
