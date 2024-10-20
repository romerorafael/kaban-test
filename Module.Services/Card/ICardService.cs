using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface ICardService
{
    Task<OneOf<Card, AppError>> Create(Card card);
    Task<OneOf<bool, AppError>> Delete(Guid guid);
    Task<OneOf<Card, AppError>> Update(Card card);
    Task<OneOf<Card, AppError>> Get(Guid cardId);
}
