﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OTSCommon.Database;

#nullable disable

namespace OTSCommon.Migrations
{
    [DbContext(typeof(OTSDbCtx))]
    [Migration("20240705211524_AddClaim")]
    partial class AddClaim
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserIdentityClaim");
                });

            modelBuilder.Entity("OTSCommon.Models.NetworkedDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OSVersion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("NetworkedDevices");
                });

            modelBuilder.Entity("OTSCommon.Models.NetworkedDeviceOwner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("NetworkedDeviceOwners");
                });

            modelBuilder.Entity("OTSCommon.Models.RoleAuthority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthLevel")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RoleAuthorities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthLevel = 0,
                            IsDefault = false,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            AuthLevel = 50,
                            IsDefault = true,
                            Name = "User"
                        },
                        new
                        {
                            Id = 3,
                            AuthLevel = 100,
                            IsDefault = true,
                            Name = "Guest"
                        });
                });

            modelBuilder.Entity("OTSCommon.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserRoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserRoleId");

                    b.ToTable("UserAccounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a06520f7-85dc-46c3-a139-d316cfc54c88",
                            Email = "",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "",
                            NormalizedUserName = "ROOT",
                            PasswordHash = "AQAAAAIAAYagAAAAEFsTpdmaz4zyTwpX4BkM/qBejQ9ZnyhsQixvGLGFco3+Rtg7INx0Zs6nUzg9XUit2w==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0887161c-0dfe-416e-b881-3aeda5508419",
                            TwoFactorEnabled = false,
                            UserName = "Root"
                        });
                });

            modelBuilder.Entity("OTSCommon.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ActiveRole")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RoleAuthorityId")
                        .HasColumnType("int");

                    b.Property<int>("UserAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleAuthorityId");

                    b.HasIndex("UserAccountId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActiveRole = true,
                            RoleAuthorityId = 1,
                            UserAccountId = 1
                        });
                });

            modelBuilder.Entity("OTSCommon.Models.NetworkedDeviceOwner", b =>
                {
                    b.HasOne("OTSCommon.Models.NetworkedDevice", "Device")
                        .WithMany("Owners")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("OTSCommon.Models.UserAccount", "Owner")
                        .WithMany("OwnedDevices")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("OTSCommon.Models.UserAccount", b =>
                {
                    b.HasOne("OTSCommon.Models.UserRole", null)
                        .WithMany("EnforcedAccounts")
                        .HasForeignKey("UserRoleId");
                });

            modelBuilder.Entity("OTSCommon.Models.UserRole", b =>
                {
                    b.HasOne("OTSCommon.Models.RoleAuthority", "RoleAuthority")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleAuthorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OTSCommon.Models.UserAccount", "UserAccount")
                        .WithMany("Roles")
                        .HasForeignKey("UserAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleAuthority");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("OTSCommon.Models.NetworkedDevice", b =>
                {
                    b.Navigation("Owners");
                });

            modelBuilder.Entity("OTSCommon.Models.RoleAuthority", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("OTSCommon.Models.UserAccount", b =>
                {
                    b.Navigation("OwnedDevices");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("OTSCommon.Models.UserRole", b =>
                {
                    b.Navigation("EnforcedAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
