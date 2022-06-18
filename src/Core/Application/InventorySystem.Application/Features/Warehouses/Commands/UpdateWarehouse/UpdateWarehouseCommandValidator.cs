using FluentValidation;

namespace InventorySystem.Application.Features.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
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
