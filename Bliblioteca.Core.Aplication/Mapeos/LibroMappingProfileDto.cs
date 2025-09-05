using AutoMapper;
using Bliblioteca.Core.Aplication.Dto;
using Bliblioteca.Core.Domain.Entity;

namespace Bliblioteca.Core.Aplication.Mapeos
{
    public class LibroMappingProfileDto : Profile
    {
        public LibroMappingProfileDto()
        {
            CreateMap<Libro, LibroDto>().ReverseMap();
        }
    }
}
