using API.Model;
using FluentValidation;

namespace Module.Services;

public class BoardValidator : AbstractValidator<Board>
{
    public BoardValidator() {
        RuleFor(b => b.Name).NotEmpty();        
    }
}
