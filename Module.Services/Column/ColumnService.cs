using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class ColumnService: IColumnService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<Column> _validator;
    private readonly IAuthService _authService;

    public ColumnService(AppDbContext dbContext, IValidator<Column> validator, IAuthService authService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public async Task<OneOf<Column, AppError>> Create(Column column)
    {
        if (column == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(column);

        if (!validation.IsValid) return new BusinessRulesError();

        Column? dbColumn = await _dbContext.Columns.FindAsync(column.Id);

        if (dbColumn != null) return new DuplicatedError();

        var newColumn = await _dbContext.Columns.AddAsync(column);
        _dbContext.SaveChanges();

        return newColumn.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(Guid guid)
    {
        var deletedColumn = await _dbContext.Columns.FindAsync(guid);

        if (deletedColumn == null) return new NotFoundError();

        var allColumnsInBoard = _dbContext.Boards.FirstOrDefault( b => b.Id == deletedColumn.BoardId);

        if (allColumnsInBoard?.Columns?.Count <= 1) return new BusinessRulesError();

        _dbContext.Columns.Remove(deletedColumn);

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<Column, AppError>> Update(Column column)
    {
        if (column == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(column);

        if (!validation.IsValid) return new BusinessRulesError();

        Column? dbColumn = await _dbContext.Columns.FindAsync(column.Id);

        if (dbColumn == null) return new NotFoundError();

        dbColumn = column;
        _dbContext.SaveChanges();

        return column;
    }

    public async Task<OneOf<List<Column>, AppError>> GetAll()
    {
        //TODO - Criar um seletor para pegar apenas os columns que o usuário tem acesso

        var columns = await _dbContext.Columns.ToListAsync();
        if (columns == null || columns.Count == 0) return new EmptyElementInsertError();

        return columns;
    }

    public async Task<OneOf<Column, AppError>> Get(long columnId)
    {
        var column = await _dbContext.Columns.SingleOrDefaultAsync(b => b.Id == columnId);

        if (column == null) return new NotFoundError();

        return column;
    }

    public async Task<OneOf<List<Column>, AppError>> GetByBoardId(long boardId)
    {
        var column = await _dbContext.Columns.Where(c => c.BoardId == boardId).ToListAsync() ;

        if (column == null) return new NotFoundError();

        return column;
    }
}
