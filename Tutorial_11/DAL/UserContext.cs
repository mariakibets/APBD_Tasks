using Microsoft.EntityFrameworkCore;
using Tutorial_11.Models;

namespace Tutorial_11.DAL;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");

        modelBuilder.Entity<User>()
            .HasMany(u => u.RefreshTokens)     
            .WithOne(rt => rt.User)            
            .HasForeignKey(rt => rt.UserId);
    }
}