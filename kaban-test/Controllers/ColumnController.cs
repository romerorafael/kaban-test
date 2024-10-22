using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controllers;

[ApiController]
[Route("column")]
public class ColumnController
{
    [HttpGet("{id}")]
    public async Task<IResult> GetById(
        [FromServices] IColumnService _columnService,
        [FromServices] IMapper _mapper,
        [FromRoute] long id
    )
    {
        var request = await _columnService.Get(id);

        return request.Match(
            column => Results.Ok(_mapper.Map<ColumnDTO>(column)),
            error =>
            {
                if (error is NotFoundError)
                    return Results.NotFound();

                return Results.BadRequest();
            }
        );
    }

    [HttpGet("byboard/{boardId}")]
    public async Task<IResult> GetByBoardId(
        [FromServices] IColumnService _columnService,
        [FromServices] IMapper _mapper,
        [FromRoute] long boardId
    )
    {
        var request = await _columnService.GetByBoardId(boardId);

        return request.Match(
            columns => Results.Ok(_mapper.Map<List<ColumnDTO>>(columns)),
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
        [FromServices] IColumnService _columnService,
        [FromServices] IMapper _mapper,
        [FromBody] ColumnDTO columnDTO
     )
    {
        Column column = _mapper.Map<Column>(columnDTO);

        var request = await _columnService.Create(column);

        return request.Match(
            column => Results.Created("/column", _mapper.Map<ColumnDTO>(column)),
            error =>
            {
                if (error is EmptyElementInsertError) return Results.NoContent();
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }

    [HttpPut]
    public async Task<IResult> Update(
        [FromServices] IColumnService _columnService,
        [FromServices] IMapper _mapper,
        [FromBody] ColumnDTO columnDTO
     )
    {
        Column column = _mapper.Map<Column>(columnDTO);

        var request = await _columnService.Update(column);

        return request.Match(
            column => Results.Created("/column", _mapper.Map<ColumnDTO>(column)),
            error =>
            {
                if (error is EmptyElementInsertError) return Results.NoContent();
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(
        [FromServices] IColumnService _columnService,
        [FromServices] IMapper _mapper,
        [FromRoute] long id
    )
    {
        var request = await _columnService.Delete(id);

        return request.Match(
            column => Results.Ok(column),
            error =>
            {
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }
}
