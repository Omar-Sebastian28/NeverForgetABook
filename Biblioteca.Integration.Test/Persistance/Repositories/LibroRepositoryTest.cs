using Bliblioteca.Core.Domain.Entity;
using Bliblioteca.Infraestructura.Persistencia.Contexts;
using Bliblioteca.Infraestructura.Persistencia.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Integration.Test.Persistance.Repositories
{
    public class LibroRepositoryTest
    {
        private readonly DbContextOptions<BibliotecaContext> _dbContextOptions;

        public LibroRepositoryTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: $"BibliotecaDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Libro_Be_Data_Base()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepositoty = new LibroRepository(context);
            var libro = new Libros
            {
                Id = 0,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Realismo Mágico",
                Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user22"
            };

            //Act.
            var result = await libroRepositoty.AddAsync(libro);

            //Assert.
            result.Should().BeTrue();
            var libroInDb = await context.Libro.ToListAsync();
            libroInDb.Should().ContainSingle();
        }


        [Fact]
        public async Task AddAsync_Should_Throw_Exception_When_Libro_Is_Null()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepositoty = new LibroRepository(context);

            //Act.
            Func<Task> act = async () => await libroRepositoty.AddAsync(null!);

            //Assert.
            await act.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Libro_When_Exists() 
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            var libro = new Libros
            {
                Id = 0,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Realismo Mágico",
                Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user22"
            };

            //Act.
            await libroRepository.AddAsync(libro);
            var (entityDb, exito) = await libroRepository.GetByIdAsync(libro.Id);

            //Assert.
            entityDb.Should().NotBeNull();
            exito.Should().BeTrue();
            entityDb!.Id.Should().Be(libro.Id);  
            entityDb.Titulo.Should().Be(libro.Titulo);
            entityDb.Autor.Should().Be(libro.Autor);
            entityDb.Genero.Should().Be(libro.Genero);
            entityDb.Descripcion.Should().Be(libro.Descripcion);
            entityDb.AñoPublicacion.Should().Be(libro.AñoPublicacion);
            entityDb.UsuarioId.Should().Be(libro.UsuarioId);
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_NotExists()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            //Act.
            var (entityDb, exito) = await libroRepository.GetByIdAsync(999);

            //Assert.
            entityDb.Should().BeNull();
            exito.Should().BeFalse();
        }


        [Fact]
        public async Task UpdateAsync_Should_Modified_Libro()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            var libro = new Libros
            {
                Id = 0,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Realismo Mágico",
                Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user22"
            };

            //Act.
            await libroRepository.AddAsync(libro);
            var updated = await libroRepository.UpdateAsync(libro, libro.Id);

            //Assert.
            updated.Should().BeTrue();           
        }


        [Fact]
        public async Task UpdateAsync_Should_Return_Null_When_Not_Found()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            var libroFake = new Libros
            {
                Id = 0,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Realismo Mágico",
                Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user22"
            };

            //Act.
            await libroRepository.AddAsync(libroFake);
            var updated = await libroRepository.UpdateAsync(libroFake, 000);

            //Assert.
            updated.Should().BeFalse();
        }


        [Fact]
        public async Task DeleteAsync_Should_Return_true_When_ExistRemove()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            var libro = new Libros
            {
                Id = 0,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                AñoPublicacion = 1967,
                Genero = "Realismo Mágico",
                Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                UsuarioId = "user22"
            };

            //Act.
            await libroRepository.AddAsync(libro);
            var deleted = await libroRepository.DeleteAsync(libro.Id);

            //Assert.
            deleted.Should().BeTrue();
        }


        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Bad_Remove()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);
            var libroRepositoty = new LibroRepository(context);

            //Act.
            var deleted = await libroRepository.DeleteAsync(33);

            //Assert.
            deleted.Should().BeFalse();
        }


        [Fact]
        public async Task GeAllListAsync_Should_Return_List_Of_Books()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);

            context.Libro.AddRange(
                new Libros
                {
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    AñoPublicacion = 1967,
                    Genero = "Realismo Mágico",
                    Descripcion = "Novela emblemática del realismo mágico que narra la historia de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo.",
                    UsuarioId = "user22"
                },
                new Libros
                {
                    Titulo = "Don Quijote de la Mancha",
                    Autor = "Miguel de Cervantes",
                    AñoPublicacion = 1605,
                    Genero = "Novela",
                    Descripcion = "Considerada la primera novela moderna, sigue las aventuras del ingenioso hidalgo Don Quijote y su fiel escudero Sancho Panza.",
                    UsuarioId = "user23"
                }
            );
            await context.SaveChangesAsync();

            //Act.
            var booksList = await libroRepository.GetAllListAsync();
            
            //Assert.
            booksList.Should().HaveCount(2);
        }

        [Fact]
        public async Task GeAllListAsync_Should_Return_When_ListOfBooks_IsEmpty()
        {
            //Arrange.
            using var context = new BibliotecaContext(_dbContextOptions);
            var libroRepository = new LibroRepository(context);

            //Act.
            var booksList = await libroRepository.GetAllListAsync();

            //Assert.
            booksList.Should().BeNullOrEmpty();
        }
    }
}

