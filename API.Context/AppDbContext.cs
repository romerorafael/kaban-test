using API.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace API.Context;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserConfig> UserConfig { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("kanban");

        modelBuilder.Entity<Board>(option =>
        {
            option.HasIndex(b => b.Id).IsCreatedConcurrently();
            option.HasMany<Column>().WithOne();
            option.HasData(SeedBoard());
        });

        modelBuilder.Entity<Card>(option =>
        {
            option.HasIndex(c => c.Id);
            option.HasOne<Board>().WithMany();
            option.HasOne<Column>().WithMany();
        });

        modelBuilder.Entity<Column>(option => { 
            option.HasIndex(c => c.Id);
            option.HasOne<Board>().WithMany();            
        });

        modelBuilder.Entity<Comment>(option => { 
            option.HasIndex(c=> c.Id).IsCreatedConcurrently();
            option.HasOne<Card>().WithMany();
            option.HasOne<User>().WithMany();
        });

        modelBuilder.Entity<Role>(option =>
        {
            option.HasIndex(r => r.Id);
            option.HasData(SeedRole());
        });

        modelBuilder.Entity<User>(option =>
        {
            option.HasIndex(u => u.Id);
            option.HasOne<Role>().WithMany();
            option.HasOne<UserConfig>()
            .WithOne(uc => uc.User);

        });
        modelBuilder.Entity<UserConfig>(option => {
            option.HasIndex(uc => uc.Icon);
            option.HasOne<User>()
            .WithOne(u => u.UserConfig);
        });        

        base.OnModelCreating(modelBuilder);
    }

    private List<Role> SeedRole()
    {
        var roles = new List<Role>();

        Role admin = new Role(
              "Admin",
              "Adiministrador do Sistema"
           );

        admin.Id = 1;
        admin.PermissionIds = new List<int>() { 0, 1, 2 };

        roles.Add(admin);

        Role user = new Role(
            "User",
            "Usuário base do Sistema"
        );

        user.Id = 2;
        user.PermissionIds = new List<int>() { 0, 1 };

        roles.Add(user);

        return roles;
    }
    private Board SeedBoard()
    {
        Board board = new Board("Bem Vindo", "Seu primeiro Quadro");
        board.Id = 1;        
        return board;
    }
}

