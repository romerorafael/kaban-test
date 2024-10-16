using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Comment(Guid cardId, Guid userId, string text) : Auditable
{
    public long Id { get; set; } = 0;
    public Guid CardId { get; set; } = cardId;
    public Guid UserId { get; set; } = userId;
    public string Text { get; set; } = text;
    public bool Edited {  get; set; } = false;

    [ForeignKey(nameof(CardId))]
    public Card? Card { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
