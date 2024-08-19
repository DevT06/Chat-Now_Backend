using System.Configuration;
using System.Data.Common;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.EntityFrameworkCore.Extensions;
using MySqlConnector;
using Shared.Entities;

namespace DataAccess.EFCore;

public class ChatNowDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ChatNowDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "server=localhost;user=cn_admin;password=Chat-Now_02.V;database=chatnow_db";

        /*options.UseMySQL(_configuration.GetConnectionString("DefaultConnection"))
            .EnableDetailedErrors();*/
        
        options.UseMySQL(connectionString)
            .EnableDetailedErrors();
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .ForMySQLHasCharset("utf8mb4")
            .ForMySQLHasCollation("utf8mb4_unicode_ci");

        modelBuilder.Entity<Message>()
            .Property(m => m.Text)
            .ForMySQLHasCharset("utf8mb4")
            .ForMySQLHasCollation("utf8mb4_unicode_ci");
    }*/
    
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User0)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.User0Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User1)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }*/
}