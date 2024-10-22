using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Column(long boardId, string name)
{
    public long Id { get; set; } = 0;
    public long BoardId { get; set; } = boardId;
    public string Name { get; set; } = name;
    public int Order { get; set; } = 1; 

    [ForeignKey(nameof(BoardId))]
    public Board? Board { get; set; }

    public ICollection<Card>? Cards { get; set; }
}
