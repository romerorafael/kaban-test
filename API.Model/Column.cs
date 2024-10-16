using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Column(long id, long boardId, string name)
{
    public long Id { get; set; } = id;
    public long BoardId { get; set; } = boardId;
    public string Name { get; set; } = name;

    [ForeignKey(nameof(BoardId))]
    public Board? Board { get; set; }
}
