﻿// <auto-generated />
using System;
using HomeNet.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HomeNet.Migrations
{
    [DbContext(typeof(NetHomeDbCtx))]
    [Migration("20240526141902_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HomeNet.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("HomeNet.Models.NetComponent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ComponentName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FirstReleaseTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MinimumRoleAuthorityId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("MinimumRoleAuthorityId");

                    b.ToTable("NetComponents");
                });

            modelBuilder.Entity("HomeNet.Models.NetControl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ComponentLibId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FunctionId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("NetComponentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NetComponentId");

                    b.ToTable("NetControls");
                });

            modelBuilder.Entity("HomeNet.Models.NetListener", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("NetComponentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("NetComponentId");

                    b.ToTable("NetListeners");
                });

            modelBuilder.Entity("HomeNet.Models.RoleAuthority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthLevel")
                        .HasColumnType("int");

                    b.Property<int?>("DowngradeId")
                        .HasColumnType("int");

                    b.Property<bool>("ForceCredential")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ReauthTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DowngradeId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HomeNet.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("RoleAuthorityId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("RoleAuthorityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HomeNet.Models.NetComponent", b =>
                {
                    b.HasOne("HomeNet.Models.RoleAuthority", "MinimumRoleAuthority")
                        .WithMany()
                        .HasForeignKey("MinimumRoleAuthorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MinimumRoleAuthority");
                });

            modelBuilder.Entity("HomeNet.Models.NetControl", b =>
                {
                    b.HasOne("HomeNet.Models.NetComponent", null)
                        .WithMany("ComponentControls")
                        .HasForeignKey("NetComponentId");
                });

            modelBuilder.Entity("HomeNet.Models.NetListener", b =>
                {
                    b.HasOne("HomeNet.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeNet.Models.NetComponent", null)
                        .WithMany("ApprovedListeners")
                        .HasForeignKey("NetComponentId");

                    b.Navigation("Device");
                });

            modelBuilder.Entity("HomeNet.Models.RoleAuthority", b =>
                {
                    b.HasOne("HomeNet.Models.RoleAuthority", "Downgrade")
                        .WithMany()
                        .HasForeignKey("DowngradeId");

                    b.Navigation("Downgrade");
                });

            modelBuilder.Entity("HomeNet.Models.UserAccount", b =>
                {
                    b.HasOne("HomeNet.Models.Device", null)
                        .WithMany("UserAccounts")
                        .HasForeignKey("DeviceId");

                    b.HasOne("HomeNet.Models.RoleAuthority", null)
                        .WithMany("Users")
                        .HasForeignKey("RoleAuthorityId");
                });

            modelBuilder.Entity("HomeNet.Models.Device", b =>
                {
                    b.Navigation("UserAccounts");
                });

            modelBuilder.Entity("HomeNet.Models.NetComponent", b =>
                {
                    b.Navigation("ApprovedListeners");

                    b.Navigation("ComponentControls");
                });

            modelBuilder.Entity("HomeNet.Models.RoleAuthority", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
