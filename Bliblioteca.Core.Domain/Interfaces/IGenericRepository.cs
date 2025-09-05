namespace Bliblioteca.Core.Domain.Interfaces
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<bool> AddAsync(Entity entity);
        Task<bool> DeleteAsync(int id);
        Task<List<Entity>> GetAllListAsync();
        Task<(Entity? entityDb, bool exito)> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Entity entity, int id);
        IQueryable<Entity> GetQuery();
    }
}