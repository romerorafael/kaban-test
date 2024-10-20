using API.Model;
using FluentValidation;

namespace Module.Services;

public class ColumnValidator : AbstractValidator<Column>
{
    public ColumnValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
    }
}
