using API.Model;
using FluentValidation;

namespace Module.Services;

public class UserConfigValidator : AbstractValidator<UserConfig>
{
    public UserConfigValidator()
    {
        RuleFor( uc => uc.UserId ).NotEmpty();
    }
}
