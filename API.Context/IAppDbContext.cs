using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public interface IAppDbContext
{
    public DbSet<Board> Boards { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserConfig> UserConfig { get; set; }

}
