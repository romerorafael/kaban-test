using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class CommentService : ICommentService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<Comment> _validator;

    public CommentService(AppDbContext dbContext, IValidator<Comment> validator)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<OneOf<Comment, AppError>> Create(Comment comment)
    {
        if (comment == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(comment);

        if (!validation.IsValid) return new BusinessRulesError();

        Comment? dbComment = await _dbContext.Comments.FindAsync(comment.Id);

        if (dbComment != null) return new DuplicatedError();

        var newComment = await _dbContext.Comments.AddAsync(comment);
        _dbContext.SaveChanges();

        return newComment.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(long id)
    {
        var deletedComment = await _dbContext.Comments.FindAsync(id);

        if (deletedComment == null) return new NotFoundError();

        _dbContext.Comments.Remove(deletedComment);
        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<Comment, AppError>> Update(Comment comment)
    {
        if (comment == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(comment);

        if (!validation.IsValid) return new BusinessRulesError();

        Comment? dbComment = await _dbContext.Comments.FindAsync(comment.Id);

        if (dbComment == null) return new NotFoundError();

        dbComment = comment;
        _dbContext.SaveChanges();

        return comment;
    }

    public async Task<OneOf<List<Comment>, AppError>> GetAll()
    {
        //TODO - Criar um seletor para pegar apenas os comments que o usuário tem acesso

        var comments = await _dbContext.Comments.ToListAsync();
        if (comments == null || comments.Count == 0) return new EmptyElementInsertError();

        return comments;
    }

    public async Task<OneOf<Comment, AppError>> Get(long commentId)
    {
        var comment = await _dbContext.Comments.SingleOrDefaultAsync(b => b.Id == commentId);

        if (comment == null) return new NotFoundError();

        return comment;
    }

    public async Task<OneOf<List<Comment>, AppError>> GetByCardId(Guid cardId)
    {
        var comment = await _dbContext.Comments.Where(c => c.CardId == cardId).ToListAsync();

        if (comment == null) return new NotFoundError();

        return comment;
    }

}
