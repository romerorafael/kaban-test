namespace API.Model;

public class CardDTO
{
    public Guid Id { get; set; }
    public long BoardId { get; set; } 
    public long ColumnId { get; set; } 
    public string? Description { get; set; }
    public DateOnly? StartAt { get; set; }
    public DateOnly? EndAt { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public List<string>? Tags { get; set; }
    public List<Guid>? Members { get; set; }
}
