using AutoMapper;
using Bliblioteca.Core.Aplication.Interfaces;
using Bliblioteca.Core.Domain.Interfaces;

namespace Bliblioteca.Core.Aplication.Services
{
    public class GenericServices<TEntity, TDtoEntity> : IGenericServices<TDtoEntity> where TEntity : class
        where TDtoEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;
        private readonly IMapper _autoMapper;

        public GenericServices(IGenericRepository<TEntity> genericRepository, IMapper autoMapper)
        {
            _genericRepository = genericRepository;
            _autoMapper = autoMapper;
        }

        public virtual async Task<bool> AddAsync(TDtoEntity dtoEntity)
        {
            if (dtoEntity is null)
            {
                return false;
            }
            try
            {
                var entity = _autoMapper.Map<TEntity>(dtoEntity);
                await _genericRepository.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servicio al agregar la entidad: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return false;
        }


        public virtual async Task<bool> UpdateAsync(TDtoEntity dtoEntity, int id)
        {
            if (dtoEntity is null || id <= 0)
            {
                return false;
            }
            try
            {
                var entity = _autoMapper.Map<TEntity>(dtoEntity);
                await _genericRepository.UpdateAsync(entity, id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servicio al actualizar la entidad: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return false;
        }

        public virtual async Task<(TDtoEntity? dtoEntity, bool exito)> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return (null,false);
            }
            try
            {              
                var entity = await _genericRepository.GetByIdAsync(id);
                if (!entity.exito || entity.entityDb is null) 
                {
                    return (null, false);
                }

                var dtoEntity = _autoMapper.Map<TDtoEntity>(entity.entityDb);
                return (dtoEntity,true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servicio al obtener la entidad por id: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return (null, false);
        }


        public virtual async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            try
            {
                await _genericRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servicio al borrar la entidad: {ex.Message}");
                if (ex.InnerException is not null)
                {
                    Console.WriteLine($"Detalle interno: {ex.InnerException.Message}");
                }
            }
            return false;
        }


        public virtual async Task<List<TDtoEntity>> GetAllListAsync()
        {
            var entity = await _genericRepository.GetAllListAsync();
            if (entity.Count == 0)
            {
                return new List<TDtoEntity>();
            }
            var entitydto = _autoMapper.Map<List<TDtoEntity>>(entity);
            return entitydto;
        }
    }
}
