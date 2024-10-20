using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class CardService: ICardService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<Card> _validator;

    public CardService(AppDbContext dbContext, IValidator<Card> validator, IAuthService authService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<OneOf<Card, AppError>> Create(Card card)
    {
        if (card == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(card);

        if (!validation.IsValid) return new BusinessRulesError();

        Card? dbCard = await _dbContext.Cards.FindAsync(card.Id);

        if (dbCard != null) return new DuplicatedError();

        var newCard = await _dbContext.Cards.AddAsync(card);
        _dbContext.SaveChanges();

        return newCard.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(Guid guid)
    {
        var deletedCard = await _dbContext.Cards.FindAsync(guid);

        if (deletedCard == null) return new NotFoundError();

        var allCardsInBoard = _dbContext.Boards.FirstOrDefault(b => b.Id == deletedCard.BoardId);

        _dbContext.Cards.Remove(deletedCard);

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<Card, AppError>> Update(Card card)
    {
        if (card == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(card);

        if (!validation.IsValid) return new BusinessRulesError();

        Card? dbCard = await _dbContext.Cards.FindAsync(card.Id);

        if (dbCard == null) return new NotFoundError();

        dbCard = card;
        _dbContext.SaveChanges();

        return card;
    }

    public async Task<OneOf<Card, AppError>> Get(Guid cardId)
    {
        var card = await _dbContext.Cards.SingleOrDefaultAsync(b => b.Id == cardId);

        if (card == null) return new NotFoundError();

        return card;
    }
}
