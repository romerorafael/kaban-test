using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class UserServices : IUserServices
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<User> _validator;
    private readonly IAuthService _authService;

    public UserServices(AppDbContext dbContext, IValidator<User> validator, IAuthService authService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public async Task<OneOf<User, AppError>> Create(User user)
    {
        if (user == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(user);

        if (!validation.IsValid) return new BusinessRulesError();

        User? dbUser = await _dbContext.Users.FindAsync(user.Id);

        if (dbUser != null) return new DuplicatedError();


        var newUser = await _dbContext.Users.AddAsync(user);
        _dbContext.SaveChanges();

        var userconfig = await CreateInitUserConfig(newUser.Entity);
        user.UserConfigId = userconfig.AsT0.Id;
        newUser.Entity.Password = _authService.CodePassword(user.Password, newUser.Entity.Id);
        _dbContext.SaveChanges();


        return newUser.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(Guid guid)
    {
        var deleterUser = await _dbContext.Users.FindAsync(guid);

        if (deleterUser == null) return new NotFoundError();

        var deletedConfig = await _dbContext.UserConfig.FirstOrDefaultAsync( uc => uc.UserId == deleterUser.Id);

        if (deletedConfig == null) return new NotFoundError();

        _dbContext.UserConfig.Remove(deletedConfig);
        _dbContext.Users.Remove(deleterUser);

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<User, AppError>> GetUserByGuidAsync(Guid guid)
    {
        User? user = await _dbContext.Users.FindAsync(guid);

        if (user == null) return new NotFoundError();

        return user;
    }

    public async Task<OneOf<List<User>, AppError>> GetUsersAsync()
    {
        try
        {
            return await _dbContext.Users.Where(u => true).ToListAsync();
        }
        catch (Exception ex)
        {
            return new BadRequestError();
        }
    }

    public async Task<OneOf<User, AppError>> Update(User user)
    {
        if (user == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(user);

        if (!validation.IsValid) return new BusinessRulesError();

        User? dbUser = await _dbContext.Users.FindAsync(user.Id);

        if (dbUser == null) return new NotFoundError();

        dbUser = user;
        _dbContext.SaveChanges();

        return user;
    }

    private async Task<OneOf<UserConfig, AppError>> CreateInitUserConfig(User user)
    {
        if (user == null) return new EmptyElementInsertError();

        var config = new UserConfig(user.Id);

        await _dbContext.UserConfig.AddAsync(config);

        return config;
    }
}
