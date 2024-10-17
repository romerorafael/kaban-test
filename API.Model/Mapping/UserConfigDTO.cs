namespace API.Model;
public class UserConfigDTO
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long NumberOfCards { get; set; }
    public string Icon { get; set; } 
    public string Color { get; set; }
}