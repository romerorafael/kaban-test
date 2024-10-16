using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Card(long boardId, long columnId, string description) : Auditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long BoardId { get; set; } = boardId;
    public long ColumnId { get; set; } = columnId;
    public string Description { get; set; } = description;
    public DateOnly? StartAt { get; set; }
    public DateOnly? EndAt { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public List<string>? Tags { get; set; }
    public List<Guid>? Members { get; set; }

    [ForeignKey(nameof(BoardId))]
    public Board? Board { get; set; }
    [ForeignKey(nameof(ColumnId))]
    public Column? Column { get; set; }
}
