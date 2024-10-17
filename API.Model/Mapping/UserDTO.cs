using System.Xml.Linq;

namespace API.Model;

public class UserDTO
{
    public Guid Id { get; set; }
    public long UserConfigId { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; } 
    public string? Email { get; set; }
    public string? Username { get; set; } 
    public long RoleId { get; set; }
}
