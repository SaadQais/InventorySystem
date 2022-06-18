using FluentValidation;

namespace InventorySystem.Application.Features.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
    {
        public CreateWarehouseCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{Name} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Name} must not exceed 50 characters.");

            RuleFor(p => p.Location)
                .NotEmpty().WithMessage("{Location} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Location} must not exceed 50 characters.");
        }
    }
}
