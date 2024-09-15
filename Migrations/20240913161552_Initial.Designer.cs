﻿// <auto-generated />
using BaseAuth.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BaseAuth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240913161552_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.ToTable("AccountRoles", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("UserUuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.HasKey("Id");

                    b.HasIndex("UserUuid")
                        .IsUnique();

                    b.HasIndex(new[] { "UserUuid" }, "FK_Account_User");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Account_Uuid")
                        .IsUnique();

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Role_Uuid")
                        .IsUnique();

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountUuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int(11)");

                    b.Property<int>("Type")
                        .HasColumnType("int(11)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "AccountUuid" }, "FK_Token_Account");

                    b.HasIndex(new[] { "Uuid" }, "UQ_Token_Uuid")
                        .IsUnique();

                    b.HasIndex(new[] { "Value" }, "UQ_Token_Value")
                        .IsUnique();

                    b.ToTable("Tokens", (string)null);
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("CHAR(100)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Uuid" }, "UQ_User_Uuid")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
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

            modelBuilder.Entity("BaseAuth.Database.Entity.Token", b =>
                {
                    b.HasOne("BaseAuth.Database.Entity.Account", "Account")
                        .WithMany("Tokens")
                        .HasForeignKey("AccountUuid")
                        .HasPrincipalKey("Uuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Token_Account");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BaseAuth.Database.Entity.Account", b =>
                {
                    b.Navigation("Tokens");
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
