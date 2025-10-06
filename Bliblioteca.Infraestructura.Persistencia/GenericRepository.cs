using Bliblioteca.Core.Domain.Interfaces;
using Bliblioteca.Infraestructura.Persistencia.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bliblioteca.Infraestructura.Persistencia
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {

        private readonly BibliotecaContext _context;

        public GenericRepository(BibliotecaContext context)
        {
            _context = context;
        }



        public virtual async Task<bool> AddAsync(Entity entity)
        {
            try
            {
                await _context.Set<Entity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new ArgumentNullException();
            }
        }



        public virtual async Task<bool> UpdateAsync(Entity entity, int id)
        {
            var entry = await _context.Set<Entity>().FindAsync(id);
            try
            {
                if (entry is not null)
                {
                    _context.Entry(entry).CurrentValues.SetValues(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error al actualizar entidad{ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return false;
        }


        public async Task<(Entity? entityDb, bool exito)> GetByIdAsync(int id)
        {
            Entity? entity = await _context.Set<Entity>().FindAsync(id);
            if (entity is not null)
            {
                return (entity, true);
            }
            else
            {
                return (null, false);
            }
        }


        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<Entity>().FindAsync(id);
                if (entity is not null)
                {
                    _context.Set<Entity>().Remove(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error al borrar la entidad: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return false;
        }


        public async Task<List<Entity>> GetAllListAsync()
        {
            return await _context.Set<Entity>().ToListAsync();
        }


        public IQueryable<Entity> GetQuery() 
        {
            return _context.Set<Entity>().AsQueryable();
        }
    }
}  
