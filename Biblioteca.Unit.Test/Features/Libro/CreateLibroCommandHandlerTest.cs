using AutoMapper;
using Bliblioteca.Core.Aplication.Features.Libro.Commands.CreateLibro;
using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Core.Domain.Interfaces;
using Bliblioteca.Infraestructura.Persistencia.Contexts;
using Bliblioteca.Infraestructura.Persistencia.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Biblioteca.Unit.Test.Features.Libro
{
    public class CreateLibroCommandHandlerTest
    {
        private readonly DbContextOptions<BibliotecaContext> dbContextOptions;
        private readonly IMapper _mapper;

        public CreateLibroCommandHandlerTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: $"BibliotecaDb_{Guid.NewGuid()}")
                .Options;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateLibroCommand, Libros>();
            }).CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Return_True_When_Creation_Is_Succesful()
        {
        //arrange
            using var context = new BibliotecaContext(dbContextOptions);
            var repository = new LibroRepository(context);
            var handler = new CreateLibroCommandHandler(repository, _mapper);
            var command = new CreateLibroCommand
            {
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Novela",
                Descripcion = "Una novela que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user123"
            };

            //act
            var result = await handler.Handle(command, CancellationToken.None);

            //assert
            result.Should().BeTrue();
        }



        [Fact]
        public async Task Handle_Should_Return_False_When_Creation_Is_Failed()
        {
            //arrange
            Mock<ILibroRepository> _mockRepository = new();
            var handler = new CreateLibroCommandHandler(_mockRepository.Object, _mapper);

            using var context = new BibliotecaContext(dbContextOptions);           
            var command = new CreateLibroCommand
            {
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Novela",
                Descripcion = "Una novela que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user123"
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Libros>()))
                .ReturnsAsync(false); 

            //act
            var result = await handler.Handle(command, CancellationToken.None);

            //assert
            result.Should().BeFalse();
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Libros>()), Times.Once);
        }
    }
}
