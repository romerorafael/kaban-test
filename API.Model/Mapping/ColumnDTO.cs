using System.Xml.Linq;

namespace API.Model;

public class ColumnDTO
{
    public long Id { get; set; }
    public long BoardId { get; set; }
    public string? Name { get; set; }
}
