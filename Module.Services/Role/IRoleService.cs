using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface IRoleService
{
    Task<OneOf<Role, AppError>> Create(Role role);
    Task<OneOf<bool, AppError>> Delete(Guid guid);
    Task<OneOf<List<Role>, AppError>> GetRolesAsync();
    Task<OneOf<Role, AppError>> Update(Role role);
}
