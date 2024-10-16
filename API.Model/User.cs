using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class User(string name, string password, string email, string username, long roleId)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long UserConfigId { get; set; } = default(long);
    public string Name { get; set; } = name;
    public string Password { get; set; } = password;
    public string Email { get; set; } = email;
    public string Username { get; set; } = username;
    public long RoleId { get; set; } = roleId;

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }

    [ForeignKey(nameof(UserConfigId))]
    public UserConfig? UserConfig { get; set; }

    ICollection<Card>? Cards { get; set; }
}
