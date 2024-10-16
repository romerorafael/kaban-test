namespace API.Model;

public class RoleDTO
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<int>? PermissionIds { get; set; }
}
