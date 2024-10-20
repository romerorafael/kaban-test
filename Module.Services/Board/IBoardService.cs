using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface IBoardService
{
    Task<OneOf<Board, AppError>> Create(Board board);
    Task<OneOf<bool, AppError>> Delete(Guid guid);
    Task<OneOf<Board, AppError>> Update(Board board);
    Task<OneOf<List<Board>, AppError>> GetAll();
    Task<OneOf<Board, AppError>> Get(long boardId);
}
