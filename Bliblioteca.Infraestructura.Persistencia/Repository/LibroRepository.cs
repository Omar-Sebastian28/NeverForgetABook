using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;
using Bliblioteca.Infraestructura.Persistencia.Contexts;

namespace Bliblioteca.Infraestructura.Persistencia.Repository
{
    public class LibroRepository : GenericRepository<Libros> ,ILibroRepository
    {
        private readonly BibliotecaContext _context;

        public LibroRepository(BibliotecaContext context) : base(context) 
        {
            _context = context;
        }
    }
}
