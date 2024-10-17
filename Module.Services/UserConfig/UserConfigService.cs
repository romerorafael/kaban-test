using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Module.Services;

internal class UserConfigService : IUserConfigService
{
    private readonly AppDbContext _appDbContext;
    private readonly IValidator<UserConfig> _validator;

    public UserConfigService(AppDbContext appDbContext, IValidator<UserConfig> validator)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(_appDbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(_validator));
    }

    public async Task<OneOf<UserConfig, AppError>> GetConfigByUserId(Guid userId)
    {
        if (userId == Guid.Empty) return new EmptyElementInsertError();

        UserConfig? config = await _appDbContext.UserConfig.FirstOrDefaultAsync(x => x.UserId == userId);

        if (config == null) return new NotFoundError();

        return config;
    }

    public async Task<OneOf<UserConfig, AppError>> Update(UserConfig userConfig)
    {
        if (userConfig == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(userConfig);

        if (!validation.IsValid) return new BusinessRulesError();

        UserConfig? dbConfig = await _appDbContext.UserConfig.FirstOrDefaultAsync(uc => uc.Id == userConfig.Id);

        if (dbConfig == null) return new NotFoundError();

        dbConfig = userConfig;

        _appDbContext.SaveChanges();

        return dbConfig;
    }
}
