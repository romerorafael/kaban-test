namespace API.Model;

public class TokenResponse
{
    public TokenResponse(string type, string token)
    {
        Type = type;
        Token = token;
    }

    public string? Type { get; }
    public string? Token { get; }
}
