using static System.Net.Mime.MediaTypeNames;

namespace API.Model;
public class CommentDTO
{
    public long Id { get; set; }
    public Guid CardId { get; set; }
    public Guid UserId { get; set; }
    public string? Text { get; set; }
    public bool Edited { get; set; } 
}

