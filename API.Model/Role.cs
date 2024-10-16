using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model;

public class Role(string name, string description)
{
    public long Id { get; set; } = default;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public List<int> PermissionIds { get; set; } = new List<int>();


    [NotMapped] // don't map this property to the database
    public List<Permission> Permissions
    {
        get { return Convert(PermissionIds); }
        set { PermissionIds = Convert(value); }
    }

    public static List<Permission> Convert(List<int> permissionIds)
    {
        return permissionIds.Select(id => (Permission)id).ToList();
    }

    public static List<int> Convert(List<Permission> permissions)
    {
        return permissions.Select(p => (int)p).ToList();
    }
}
