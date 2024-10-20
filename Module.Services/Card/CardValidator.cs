using API.Model;
using FluentValidation;

namespace Module.Services;
internal class CardValidator: AbstractValidator<Card>
{
    public CardValidator()
    {
        RuleFor(c => c.BoardId).NotEmpty();
    }
}
