using FluentValidation;

namespace TaskWSS.ViewModels;

public class CreateDepartmentRequest
{
    public string Name { get; set; }
    
    public int CompanyId { get; set; }
}

public sealed class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CompanyId).NotEmpty().WithMessage("CompanyId is required");
    }
}