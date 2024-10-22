using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface IColumnService
{
    Task<OneOf<Column, AppError>> Create(Column column);
    Task<OneOf<bool, AppError>> Delete(long id);
    Task<OneOf<Column, AppError>> Update(Column column);
    Task<OneOf<List<Column>, AppError>> GetAll();
    Task<OneOf<Column, AppError>> Get(long columnId);
    Task<OneOf<List<Column>, AppError>> GetByBoardId(long boardId);
}
