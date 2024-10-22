using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controllers;

[ApiController]
[Route("card")]
public class CardController
{
    [HttpGet("{id}")]
    public async Task<IResult> GetById(
        [FromServices] ICardService _cardService,
        [FromServices] IMapper _mapper,
        [FromRoute] Guid id
    )
    {
        var request = await _cardService.Get(id);

        return request.Match(
            card => Results.Ok(_mapper.Map<CardDTO>(card)),
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
        [FromServices] ICardService _cardService,
        [FromServices] IMapper _mapper,
        [FromBody] CardDTO cardDTO
     )
    {
        Card card = _mapper.Map<Card>(cardDTO);

        var request = await _cardService.Create(card);

        return request.Match(
            card => Results.Created("/card", _mapper.Map<CardDTO>(card)),
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
        [FromServices] ICardService _cardService,
        [FromServices] IMapper _mapper,
        [FromBody] CardDTO cardDTO
     )
    {
        Card card = _mapper.Map<Card>(cardDTO);

        var request = await _cardService.Update(card);

        return request.Match(
            card => Results.Created("/card", _mapper.Map<CardDTO>(card)),
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
        [FromServices] ICardService _cardService,
        [FromServices] IMapper _mapper,
        [FromRoute] Guid boardId
    )
    {
        var request = await _cardService.Delete(boardId);

        return request.Match(
            card => Results.Ok(card),
            error =>
            {
                if (error is BusinessRulesError || error is DuplicatedError) return Results.UnprocessableEntity();

                return Results.BadRequest();
            }
        );
    }
}
