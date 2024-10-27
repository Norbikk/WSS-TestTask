using FluentValidation;

namespace TaskWSS.ViewModels;

public class CreateCompanyRequest
{
    public string Name { get; set; }
}

public sealed class CreateCompanyRequestRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}