using API.Model;
using FluentValidation;

namespace Module.Services;

internal class RoleValidator: AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Permissions).NotEmpty();
    }
}
