using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface ICommentService
{
    Task<OneOf<Comment, AppError>> Create(Comment comment);
    Task<OneOf<bool, AppError>> Delete(long id);
    Task<OneOf<Comment, AppError>> Update(Comment comment);
    Task<OneOf<List<Comment>, AppError>> GetAll();
    Task<OneOf<Comment, AppError>> Get(long commentId);
    Task<OneOf<List<Comment>, AppError>> GetByCardId(Guid cardId);
}
