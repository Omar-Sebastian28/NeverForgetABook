using AutoMapper;
using Bliblioteca.Core.Aplication.Dto.Libro;
using Bliblioteca.Core.Aplication.Mapeos;
using Bliblioteca.Core.Aplication.Services;
using Bliblioteca.Infraestructura.Persistencia.Contexts;
using Bliblioteca.Infraestructura.Persistencia.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Unit.Test.Services
{
    public class LibroServiceTests
    {
        private readonly DbContextOptions<BibliotecaContext> _dbContextOptions;
        private readonly IMapper _mapper;

        public LibroServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LibroMappingProfileDto>(); 
            });
            _mapper = config.CreateMapper();
        }

        private LibroServices CreateServices()
        {
            var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            return new LibroServices(libroRepository, _mapper);
        }


        [Fact]
        public async Task AddAsync_Should_Return_True_When_Add_New_Libro()
        {
            // Arrange
            var service = CreateServices();
            var newLibro = new LibroDto
            {
                Titulo = "Test Libro",
                Autor = "Test Autor",
                AñoPublicacion = 2023,
                Genero = "Test Genero",
                Descripcion = "Test Descripcion",
            };

            // Act
            var result = await service.AddAsync(newLibro);

            // Assert
            result.Should().BeTrue();      
        }



        [Fact]
        public async Task AddAsync_Should_Return_False_When_Add_NullBook() 
        {        
            //Arrange
            var service = CreateServices();

            // Act
            var result = await service.AddAsync(null!);

            // Assert
            result.Should().BeFalse();
        }



        [Fact]
        public async Task GetByIdAsync_Should_Return_LibroDto_When_Libro_Exists()
        {
            // Arrange
            var service = CreateServices();
            var newLibro = new LibroDto
            {
                Id = 0,
                Titulo = "Test Libro",
                Autor = "Test Autor",
                AñoPublicacion = 2023,
                Genero = "Test Genero",
                Descripcion = "Test Descripcion",
            };

            await service.AddAsync(newLibro);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.exito.Should().BeTrue();
            result.dtoEntity!.Id.Should().Be(1);
            result.dtoEntity.Titulo.Should().Be("Test Libro");
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Libro_NotExists()
        {
            // Arrange
            var service = CreateServices();

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            result.dtoEntity.Should().BeNull();
            result.exito.Should().BeFalse();
        }


        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Remove_Is_Completed()
        {
            // Arrange
            var context = new BibliotecaContext(_dbContextOptions);
            var service = CreateServices();
            var newLibro = new LibroDto
            {
                Id = 0,
                Titulo = "Test Libro",
                Autor = "Test Autor",
                AñoPublicacion = 2023,
                Genero = "Test Genero",
                Descripcion = "Test Descripcion",
            };

            await service.AddAsync(newLibro);
            var searchEntity = await context.Libro.FirstOrDefaultAsync(b => b.Titulo == "Test Libro");

            newLibro.Id = searchEntity!.Id;
            newLibro.Titulo = searchEntity.Titulo = "titulo modificado.";

            // Act
            var result = await service.DeleteAsync(newLibro.Id);

            // Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Remove_Is_Failed()
        {
            // Arrange
            var service = CreateServices();

            // Act
            var result = await service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public async Task UpdateAsync_Should_Return_True_When_Modified_Is_Completed()
        {
            // Arrange
            var context = new BibliotecaContext(_dbContextOptions);
            var service = CreateServices();
            var libro = new LibroDto
            {
                Id = 0,
                Titulo = "Test Libro",
                Autor = "Test Autor",
                AñoPublicacion = 2023,
                Genero = "Test Genero",
                Descripcion = "Test Descripcion",
            };

            // Act
            await service.AddAsync(libro);
            var searchEntity = await context.Libro.FirstOrDefaultAsync(b => b.Titulo == "Test Libro");

            libro.Id = searchEntity!.Id;
            libro.Titulo = searchEntity.Titulo = "titulo modificado.";
                       
            var result = await service.UpdateAsync(libro, libro.Id);

            // Assert
            result.Should().BeTrue();
        }


        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Modified_Is_Failed()
        {
            // Arrange
            var service = CreateServices();

            // Act
            var result = await service.UpdateAsync(null!, 999);

            // Assert
            result.Should().BeFalse();
        }




        [Fact]
        public async Task GetAllAsync_Should_Return_All_Libros()
        {
            // Arrange
            var service = CreateServices();
            var libro1 = new LibroDto
            {
                Titulo = "Libro 1",
                Autor = "Autor 1",
                AñoPublicacion = 2020,
                Genero = "Genero 1",
                Descripcion = "Descripcion 1",
            };
            var libro2 = new LibroDto
            {
                Titulo = "Libro 2",
                Autor = "Autor 2",
                AñoPublicacion = 2021,
                Genero = "Genero 2",
                Descripcion = "Descripcion 2",
            };

            await service.AddAsync(libro1);
            await service.AddAsync(libro2);

            // Act
            var result = await service.GetAllListAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }



        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_List_IsEmpty()
        {
            // Arrange
            var service = CreateServices();

            // Act
            var result = await service.GetAllListAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
