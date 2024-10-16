using API.Context;
using API.Model;
using API.OneOfErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Module.Auth;

internal class AuthService : IAuthService
{
    private readonly TokenParams _tokenParams;
    private readonly AppDbContext _dbContext;
    private const int _HOURS_TO_EXPIRE_TOKEN = 2;

    public AuthService(TokenParams tokenParams, AppDbContext dbContext)
    {
        _tokenParams = tokenParams ?? throw new ArgumentNullException(nameof(tokenParams));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public OneOf<TokenResponse, AppError> AuthenticateUser(User user)
    {
        if (user == null) return new NullError();

        var dbUser = _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Id == user.Id);
        
        if (dbUser == null) return new NullError();

        bool isVerify = VerifyPassword(user.Password, dbUser.Password, dbUser.Id);

        if (!isVerify) return new UnauthorizadedError();

        TokenResponse? token = GenerateToken(dbUser);

        return token;
    }

    public async Task<OneOf<bool, AppError>> RestartPassword(RestartPassword restartPassword)
    {
        if (restartPassword == null) return new NullError();

        User user = await _dbContext.Users.FirstAsync( u => u.Id == restartPassword.UserId);

        if (user == null) return new NullError();

        var oldPasswordCoded = CodePassword(restartPassword.OldPassword, user.Id);

        if (user.Password != oldPasswordCoded) return new UnauthorizadedError();

        var newPasswordCoded = CodePassword(restartPassword.NewPassword, user.Id);

        user.Password = newPasswordCoded;

        _dbContext.SaveChanges();

        return true;
    }

    public bool ValidateToken(string token, string privateKey, out ClaimsPrincipal? principal)
    {
        principal = null;

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(privateKey);

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = false, // Defina como true se deseja validar o emissor
            ValidateAudience = false, // Defina como true se deseja validar o público-alvo
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            SecurityToken validatedToken;
            principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            // Em caso de falha na validação (token inválido, expirado, etc.), você pode lidar com a exceção aqui
            Console.WriteLine($"Erro na validação do token: {ex.Message}");
            return false;
        }
    }

    public string CodePassword(string password, Guid guid)
    {
        using var sha256 = SHA256.Create();
        // Gera um GUID aleatório sem hifens
        string salt = guid.ToString().Replace("-", "");

        // Combina a senha com o salt
        string withsalt = $"{password}{salt}";

        var byteValue = Encoding.UTF8.GetBytes(withsalt);
        var byteHash = sha256.ComputeHash(byteValue);
        var hash = Convert.ToBase64String(byteHash);

        // Retorna a senha codificada: hash + salt
        return hash;
    }

    private TokenResponse GenerateToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_tokenParams.PrivateKey!);
        SecurityTokenDescriptor tokenSpecificationDescriptor = DescribeTokenSpecification(user, key);
        SecurityToken securityToken = tokenHandler.CreateToken(tokenSpecificationDescriptor);
        string token = tokenHandler.WriteToken(securityToken);
        return new TokenResponse("Bearer", token);
    }

    private static SecurityTokenDescriptor DescribeTokenSpecification(User user, byte[] key)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = ConfigureClaimsIdentity(user),
            Expires = DateTime.UtcNow.AddHours(_HOURS_TO_EXPIRE_TOKEN),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenDescriptor;
    }

    private bool VerifyPassword(string passwordIn, string dbPassword, Guid hash)
    {
        var codePasswordIn = CodePassword(passwordIn, hash);

        return dbPassword == codePasswordIn;
    }

    private static ClaimsIdentity ConfigureClaimsIdentity(User user)
    {
        var permisisons = user.Role.Permissions.Select(x =>
        {
            return x.ToString();
        });

        var permisisonString = String.Join(",", permisisons);

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Authentication, permisisonString),
            new Claim(ClaimTypes.Expiration, DateTime.Now.AddHours(_HOURS_TO_EXPIRE_TOKEN).ToString())
        ];

        ClaimsIdentity claimsIdentity = new(claims);

        return claimsIdentity;
    }
}
