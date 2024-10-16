namespace API.Model;

public class Board(string name, string description)
{
    public long Id { get; set; } = 0;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public string Icon { get; set; } = "board";
    public string Color { get; set; } = "blue";

    public ICollection<Column>? Columns { get; set; }
}
