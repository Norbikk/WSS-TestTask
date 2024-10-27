using FluentValidation;

namespace TaskWSS.ViewModels;

public class CreateUnitDepartmentRequest
{
    public string Name { get; set; }
    
    public int DepartmentId { get; set; }
}

public sealed class CreateUnitDepartmentRequestValidator : AbstractValidator<CreateUnitDepartmentRequest>
{
    public CreateUnitDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required");
    }
}