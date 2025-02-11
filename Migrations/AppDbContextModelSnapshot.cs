﻿// <auto-generated />
using BaseAuth.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BaseAuth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.Property<int>("AccountsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("AccountsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("account_roles", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasColumnName("password");

                    b.Property<string>("UserUuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)")
                        .HasColumnName("user_uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("username");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("(UUID())");

                    b.HasKey("Id");

                    b.HasAlternateKey("Uuid");

                    b.HasIndex("UserUuid")
                        .IsUnique();

                    b.HasIndex(new[] { "UserUuid" }, "FK_Account_User");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Account_Uuid")
                        .IsUnique();

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("name");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("(UUID())");

                    b.HasKey("Id");

                    b.HasAlternateKey("Uuid");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Role_Uuid")
                        .IsUnique();

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("CHAR(36)")
                        .HasColumnName("uuid")
                        .HasDefaultValueSql("(UUID())");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Uuid" }, "UQ_User_Uuid")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("AccountRole", b =>
                {
                    b.HasOne("BaseAuth.Database.Entity.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseAuth.Database.Entity.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Account", b =>
                {
                    b.HasOne("BaseAuth.Database.Entity.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("BaseAuth.Database.Entity.Account", "UserUuid")
                        .HasPrincipalKey("BaseAuth.Database.Entity.User", "Uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Account_User");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.User", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
