namespace Bliblioteca.Core.Aplication.Interfaces
{
    public interface IGenericServices<TDtoEntity>
        where TDtoEntity : class
    {
        Task<bool> AddAsync(TDtoEntity dtoEntity);
        Task<bool> UpdateAsync(TDtoEntity dtoEntity, int id);
        Task<bool> DeleteAsync(int id);
        Task<List<TDtoEntity>> GetAllListAsync();
        Task<(TDtoEntity? dtoEntity,bool exito)> GetByIdAsync(int id);       
    }
}