using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class BoardService : IBoardService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<Board> _validator;
    private readonly IAuthService _authService;

    public BoardService(AppDbContext dbContext, IValidator<Board> validator, IAuthService authService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public async Task<OneOf<Board, AppError>> Create(Board board)
    {
        if (board == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(board);

        if (!validation.IsValid) return new BusinessRulesError();

        Board? dbBoard = await _dbContext.Boards.FindAsync(board.Id);

        if (dbBoard != null) return new DuplicatedError();

        var newBoard = await _dbContext.Boards.AddAsync(board);
        await CreateInitBoard(newBoard.Entity.Id);
        _dbContext.SaveChanges();

        return newBoard.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(Guid guid)
    {
        var deleterBoard = await _dbContext.Boards.FindAsync(guid);

        if (deleterBoard == null) return new NotFoundError();

        var allBoards = _dbContext.Boards.ToList();
        if (allBoards.Count <= 1) return new BusinessRulesError();

        _dbContext.Boards.Remove(deleterBoard);

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<Board, AppError>> Update(Board board)
    {
        if (board == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(board);

        if (!validation.IsValid) return new BusinessRulesError();

        Board? dbBoard = await _dbContext.Boards.FindAsync(board.Id);

        if (dbBoard == null) return new NotFoundError();

        dbBoard = board;
        _dbContext.SaveChanges();

        return board;
    }

    public async Task<OneOf<List<Board>, AppError>> GetAll()
    {
        //TODO - Criar um seletor para pegar apenas os boards que o usuário tem acesso

        var boards = await _dbContext.Boards.ToListAsync();
        if(boards == null || boards.Count == 0) return new EmptyElementInsertError();

        return boards;
    }

    public async Task<OneOf<Board, AppError>> Get(long boardId)
    {
        var board = await _dbContext.Boards.SingleOrDefaultAsync(b => b.Id == boardId);

        if (board == null) return new NotFoundError();

        return board;
    }

    private async Task<OneOf<Column, AppError>> CreateInitBoard(long boardId)
    {
        if (boardId == default(long)) return new EmptyElementInsertError();

        var column = new Column(boardId, "Nova Coluna");

        var newColumn = await _dbContext.Columns.AddAsync(column);
        await _dbContext.SaveChangesAsync();

        return newColumn.Entity;
    }

}
