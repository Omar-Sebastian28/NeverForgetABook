namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface IGenericServices<DtoEntity>
        where DtoEntity : class
    {
        Task<bool> AddAsync(DtoEntity dtoEntity);
        Task<bool> UpdateAsync(DtoEntity dtoEntity, int id);
        Task<bool> DeleteAsync(int id);
        Task<List<DtoEntity>> GetAllListAsync();
        Task<(DtoEntity? DtoEntity, bool exito)> GetByIdAsync(int id);       
    }
}