using API.Model;
using API.OneOfErrors;
using OneOf;

namespace Module.Services;

public interface IUserConfigService
{
    Task<OneOf<UserConfig, AppError>> GetConfigByUserId(Guid userId);
    Task<OneOf<UserConfig, AppError>> Update(UserConfig userConfig);
}
