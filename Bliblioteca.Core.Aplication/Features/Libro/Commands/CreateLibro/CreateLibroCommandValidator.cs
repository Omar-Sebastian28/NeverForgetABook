using FluentValidation;

namespace Bliblioteca.Core.Aplication.Features.Libro.Commands.CreateLibro
{
    public class CreateLibroCommandValidator : AbstractValidator<CreateLibroCommand>
    {
        public CreateLibroCommandValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres.");


            RuleFor(x => x.Autor)
                .NotEmpty().WithMessage("El autor es obligatorio.")
                .MaximumLength(100).WithMessage("El autor no puede exceder los 100 caracteres.");

        
            RuleFor(x => x.AñoPublicacion)
                .InclusiveBetween(2000, DateTime.Now.Year).WithMessage($"El año de publicación debe estar entre 1450 y {DateTime.Now.Year}.");


            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");


            RuleFor(x => x.Genero)
                .NotEmpty().WithMessage("El género es obligatorio.")
                .MaximumLength(50).WithMessage("El género no puede exceder los 50 caracteres.");
        }
    }
}
