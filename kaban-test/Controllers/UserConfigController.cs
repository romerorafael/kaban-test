using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controllers;

[ApiController]
[Route("userconfig")]
public class UserConfigController
{
    [HttpGet("bycard/{userId}")]
    public async Task<IResult> GetByCardId(
        [FromServices] IUserConfigService _userConfigService,
        [FromServices] IMapper _mapper,
        [FromRoute] Guid userId
    )
    {
        var request = await _userConfigService.GetConfigByUserId(userId);

        return request.Match(
            userConfig => Results.Ok(_mapper.Map<List<UserConfigDTO>>(userConfig)),
            error =>
            {
                if (error is NotFoundError) return Results.NotFound();
                if (error is EmptyElementInsertError) return Results.NoContent();

                return Results.BadRequest();
            }
        );
    }

    [HttpPut]
    public async Task<IResult> Update(
        [FromServices] IUserConfigService _userConfigService,
        [FromServices] IMapper _mapper,
        [FromBody] UserConfigDTO userConfigDTO
     )
    {
        UserConfig userConfig = _mapper.Map<UserConfig>(userConfigDTO);

        var request = await _userConfigService.Update(userConfig);

        return request.Match(
            userConfig => Results.Created("/userConfig", _mapper.Map<UserConfigDTO>(userConfig)),
            error =>
            {
                if (error is EmptyElementInsertError) return Results.NoContent();
                if (error is BusinessRulesError) return Results.UnprocessableEntity();
                if (error is NotFoundError) return Results.NotFound();

                return Results.BadRequest();
            }
        );
    }
}
