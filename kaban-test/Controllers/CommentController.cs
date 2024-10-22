using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controllers;

[ApiController]
[Route("comment")]
public class CommentController
{
    [HttpGet]
    public async Task<IResult> GetAll(
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper
    )
    {
        var request = await _commentService.GetAll();

        return request.Match(
            comments => Results.Ok(_mapper.Map<IEnumerable<UserDTO>>(comments)),
            error => {
                if (error is EmptyElementInsertError)
                    return Results.NoContent();

                return Results.BadRequest();
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper,
        [FromRoute] long id
    )
    {
        var request = await _commentService.Get(id);

        return request.Match(
            comment => Results.Ok(_mapper.Map<CommentDTO>(comment)),
            error =>
            {
                if (error is NotFoundError)
                    return Results.NotFound();

                return Results.BadRequest();
            }
        );
    }

    [HttpGet("bycard/{cardId}")]
    public async Task<IResult> GetByCardId(
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper,
        [FromRoute] Guid cardId
    )
    {
        var request = await _commentService.GetByCardId(cardId);

        return request.Match(
            comment => Results.Ok(_mapper.Map<List<CommentDTO>>(comment)),
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
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper,
        [FromBody] CommentDTO commentDTO
     )
    {
        Comment comment = _mapper.Map<Comment>(commentDTO);

        var request = await _commentService.Create(comment);

        return request.Match(
            comment => Results.Created("/comment", _mapper.Map<CommentDTO>(comment)),
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
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper,
        [FromBody] CommentDTO commentDTO
     )
    {
        Comment comment = _mapper.Map<Comment>(commentDTO);

        var request = await _commentService.Update(comment);

        return request.Match(
            comment => Results.Created("/comment", _mapper.Map<CommentDTO>(comment)),
            error =>
            {
                if (error is EmptyElementInsertError) return Results.NoContent();
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }

    [HttpDelete("{commentId}")]
    public async Task<IResult> Delete(
        [FromServices] ICommentService _commentService,
        [FromServices] IMapper _mapper,
        [FromRoute] long commentId
    )
    {
        var request = await _commentService.Delete(commentId);

        return request.Match(
            comment => Results.Ok(comment),
            error =>
            {
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }
}
