using API.Context;
using API.Model;
using API.OneOfErrors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Module.Auth;
using OneOf;

namespace Module.Services;

internal class RoleService : IRoleService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<Role> _validator;
    private readonly IAuthService _authService;

    public RoleService(AppDbContext dbContext, IValidator<Role> validator, IAuthService authService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public async Task<OneOf<Role, AppError>> Create(Role role)
    {
        if (role == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(role);

        if (!validation.IsValid) return new BusinessRulesError();

        Role? dbRole = await _dbContext.Roles.FindAsync(role.Id);

        if (dbRole != null) return new DuplicatedError();


        var newRole = await _dbContext.Roles.AddAsync(role);
        return newRole.Entity;
    }

    public async Task<OneOf<bool, AppError>> Delete(Guid guid)
    {
        var deleterRole = await _dbContext.Roles.FindAsync(guid);

        if (deleterRole == null) return new NotFoundError();

        _dbContext.Roles.Remove(deleterRole);

        _dbContext.SaveChanges();

        return true;
    }

    public async Task<OneOf<List<Role>, AppError>> GetRolesAsync()
    {
        try
        {
            return await _dbContext.Roles.Where(u => true).ToListAsync();
        }
        catch (Exception ex)
        {
            return new BadRequestError();
        }
    }

    public async Task<OneOf<Role, AppError>> Update(Role role)
    {
        if (role == null) return new EmptyElementInsertError();

        var validation = _validator.Validate(role);

        if (!validation.IsValid) return new BusinessRulesError();

        Role? dbRole = await _dbContext.Roles.FindAsync(role.Id);

        if (dbRole == null) return new NotFoundError();

        dbRole = role;
        _dbContext.SaveChanges();

        return role;
    }
}
