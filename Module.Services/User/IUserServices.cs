using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface IUserServices
{
    Task<OneOf<List<User>, AppError>> GetUsersAsync();
    Task<OneOf<User, AppError>> GetUserByGuidAsync(Guid guid);
    Task<OneOf<User, AppError>> Create(User user);
    Task<OneOf<User, AppError>> Update(User user);
    Task<OneOf<bool, AppError>> Delete(Guid guid);
}
