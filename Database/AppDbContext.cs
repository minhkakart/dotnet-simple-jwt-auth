using BaseAuth.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace BaseAuth.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Uuid, "UQ_User_Uuid")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("int(11)")
                .IsRequired();
            entity.Property(e => e.Uuid)
                .HasColumnName("uuid")
                .HasColumnType("CHAR(36)")
                .HasDefaultValueSql("(UUID())")
                .IsRequired();
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            entity.HasOne(e => e.Account)
                .WithOne(e => e.User)
                .HasForeignKey<Account>(e => e.UserUuid)
                .HasPrincipalKey<User>(e => e.Uuid)
                .HasConstraintName("FK_User_Account");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.UserUuid, "FK_Account_User");
            entity.HasIndex(e => e.Uuid, "UQ_Account_Uuid")
                .IsUnique();

            entity.Property(e => e.Uuid)
                .HasColumnName("uuid")
                .HasColumnType("CHAR(36)")
                .HasDefaultValueSql("(UUID())")
                .IsRequired();
            entity.Property(e => e.UserUuid)
                .HasColumnName("user_uuid")
                .HasColumnType("CHAR(100)")
                .IsRequired();
            entity.Property(e => e.Username)
                .HasColumnName("username")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            entity.Property(e => e.Password)
                .HasColumnName("password")
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

            entity.HasOne(e => e.User)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(e => e.UserUuid)
                .HasPrincipalKey<User>(e => e.Uuid)
                .HasConstraintName("FK_Account_User");

            entity.HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .UsingEntity(j => j.ToTable("account_roles"))
                .HasAlternateKey(e => e.Uuid);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Uuid, "UQ_Role_Uuid")
                .IsUnique();

            entity.Property(e => e.Uuid)
                .HasColumnName("uuid")
                .HasColumnType("CHAR(36)")
                .HasDefaultValueSql("(UUID())")
                .IsRequired();
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            entity.HasMany(e => e.Accounts)
                .WithMany(e => e.Roles)
                .UsingEntity(j => j.ToTable("account_roles"))
                .HasAlternateKey(e => e.Uuid);
        });
        
    }
}