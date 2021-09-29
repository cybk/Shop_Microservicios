using FluentValidation;

namespace Ordering.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName).NotEmpty()
                .WithMessage("El nombre de usuario no puede ir vacio")
                .NotNull().WithMessage("El nombre de usuario no puede ser NULL")
                .MaximumLength(40).WithMessage("El nombre de usuario no puede ser mayor a 40 caracteres");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("El campo de email no puede estar vacío")
                .EmailAddress().WithMessage("El campo de email debe ser una dirección valida");
        }
    }
}