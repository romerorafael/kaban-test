namespace API.Model;

public class RestartPassword
{
    public RestartPassword(Guid userId, string newPassword, string oldPassword)
    {
        UserId = userId;
        NewPassword = newPassword;
        OldPassword = oldPassword;
    }

    public required Guid UserId { get; set; }
    public required string NewPassword { get; set; }
    public required string OldPassword { get; set; }
}
