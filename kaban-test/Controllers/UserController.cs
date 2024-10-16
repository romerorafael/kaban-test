using API.Model;
using API.OneOfErrors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Module.Services;

namespace kaban_test.Controller;

[ApiController]
[Route("user")]
public class UserController
{
    [HttpGet]
    public async Task<IResult> GetUsers(
        [FromServices] IMapper _mapper ,
        [FromServices] IUserServices _userServices
    )
    {
        var resultRequest = await _userServices.GetUsersAsync();

        return resultRequest.Match(
            users => Results.Ok( _mapper.Map<IEnumerable<UserDTO>>(users)),
            error => { return Results.BadRequest(); }
        );
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetUser(
        [FromServices] IMapper _mapper,
        [FromServices] IUserServices _userServices,
        [FromRoute] Guid id
    )
    {
        var resultRequest = await _userServices.GetUserByGuidAsync(id);

        return resultRequest.Match(
            user => Results.Ok(_mapper.Map<UserDTO>(user)),
            error => { return Results.BadRequest(); }
        );
    }

    [HttpPost]
    public async Task<IResult> Create(
        [FromServices] IMapper _mapper,
        [FromServices] IUserServices _userServices,
        [FromBody] UserDTO userDTO
    )
    {
        if (userDTO == null) return Results.NoContent();

        var user = _mapper.Map<User>( userDTO );

        var resultRequest = await _userServices.Create( user );

        return resultRequest.Match(
            user => Results.Created("/users", user),
            error => { 

                if ( error is EmptyElementInsertError)
                    return Results.NoContent();
                if ( error is BusinessRulesError)
                    return Results.Conflict(error.detail);
                if ( error is DuplicatedError)
                    return Results.Conflict(error.detail);

                return Results.BadRequest(); 
            }
        );
    }

    [HttpPut]
    public async Task<IResult> Update(
        [FromServices] IMapper _mapper,
        [FromServices] IUserServices _userServices,
        [FromBody] UserDTO userDTO
    )
    {
        if (userDTO == null) return Results.NoContent();

        var user = _mapper.Map<User>(userDTO);

        var resultRequest = await _userServices.Update(user);

        return resultRequest.Match(
            user => Results.Created("/users", user),
            error => {

                if (error is EmptyElementInsertError)
                    return Results.NoContent();
                if (error is BusinessRulesError)
                    return Results.Conflict(error.detail);
                if (error is DuplicatedError)
                    return Results.Conflict(error.detail);

                return Results.BadRequest();
            }
        );
    }

}

