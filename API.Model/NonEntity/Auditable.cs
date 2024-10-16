namespace API.Model;

public class Auditable
{
    public Guid CreatedBy { get; set; } 
    public Guid UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
