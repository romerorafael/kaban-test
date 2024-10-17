using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class UserConfig(Guid userId)
{
    public long Id { get; set; } = default(long);
    public Guid UserId { get; set; } = userId;
    public long NumberOfCards { get; set; } = 0;
    public string Icon { get; set; } = "user";
    public string Color { get; set; } = "blue";


    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
