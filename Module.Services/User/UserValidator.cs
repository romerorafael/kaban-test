using API.Model;
using FluentValidation;

namespace Module.Services;

internal class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Email).NotEmpty();
        RuleFor(u => u.Name).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
        RuleFor(u => u.Username).NotEmpty();
    }
}