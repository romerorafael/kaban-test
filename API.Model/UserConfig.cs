using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class UserConfig(Guid userId, string? icon, string? color)
{
    public long Id { get; set; } = default(long);
    public Guid UserId { get; set; } = userId;
    public long NumberOfCards { get; set; } = 0;
    public string Icon { get; set; } = icon ?? "user";
    public string Color { get; set; } = color ?? "blue";


    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
