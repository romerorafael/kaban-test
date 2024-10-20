using API.Model;
using FluentValidation;

namespace Module.Services;

public class CommentValidator: AbstractValidator<Comment>
{
    public CommentValidator()
    {
        RuleFor(c => c.Text).NotEmpty();
        RuleFor(c => c.CardId).NotEmpty();
    }
}
