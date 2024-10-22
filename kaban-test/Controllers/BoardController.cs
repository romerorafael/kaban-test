using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controllers;

[ApiController]
[Route("board")]
public class BoardController
{
    [HttpGet]
    public async Task<IResult> GetAll(
        [FromServices] IBoardService _boardService,
        [FromServices] IMapper _mapper
    )
    {
        var request = await _boardService.GetAll();

        return request.Match(
            boards => Results.Ok(_mapper.Map<IEnumerable<UserDTO>>(boards)),
            error => { 
                if (error is EmptyElementInsertError)
                    return Results.NoContent();

                return Results.BadRequest(); 
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(
        [FromServices] IBoardService _boardService,
        [FromServices] IMapper _mapper,
        [FromRoute] long id
    )
    {
        var request = await _boardService.Get(id);

        return request.Match(
            board => Results.Ok(_mapper.Map<BoardDTO>(board)),
            error =>
            {
                if (error is NotFoundError)
                    return Results.NotFound();

                return Results.BadRequest();
            }
        );
    }

    [HttpPost]
    public async Task<IResult> Create(
        [FromServices] IBoardService _boardService,
        [FromServices] IMapper _mapper,
        [FromBody] BoardDTO boardDTO
     )
    {
        Board board = _mapper.Map<Board>(boardDTO);
        
        var request = await _boardService.Create(board);

        return request.Match(
            board => Results.Created("/board", _mapper.Map<BoardDTO>(board)),
            error =>
            {
                if ( error is EmptyElementInsertError ) return Results.NoContent();
                if ( error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }

    [HttpPut]
    public async Task<IResult> Update(
        [FromServices] IBoardService _boardService,
        [FromServices] IMapper _mapper,
        [FromBody] BoardDTO boardDTO
     )
    {
        Board board = _mapper.Map<Board>(boardDTO);

        var request = await _boardService.Update(board);

        return request.Match(
            board => Results.Created("/board", _mapper.Map<BoardDTO>(board)),
            error =>
            {
                if (error is EmptyElementInsertError) return Results.NoContent();
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }

    [HttpDelete("{boardId}")]
    public async Task<IResult> Delete(
        [FromServices] IBoardService _boardService,
        [FromServices] IMapper _mapper,
        [FromRoute] long boardId
    )
    {
        var request = await _boardService.Delete(boardId);

        return request.Match(
            board => Results.Ok(board),
            error =>
            {
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }
}
