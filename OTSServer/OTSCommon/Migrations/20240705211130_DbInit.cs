using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OTSCommon.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NetworkedDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<string>(type: "longtext", nullable: false),
                    DeviceName = table.Column<string>(type: "longtext", nullable: false),
                    OSVersion = table.Column<string>(type: "longtext", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkedDevices", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoleAuthorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    AuthLevel = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAuthorities", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NetworkedDeviceOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkedDeviceOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworkedDeviceOwners_NetworkedDevices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "NetworkedDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UserRoleId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleAuthorityId = table.Column<int>(type: "int", nullable: false),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    ActiveRole = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_RoleAuthorities_RoleAuthorityId",
                        column: x => x.RoleAuthorityId,
                        principalTable: "RoleAuthorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "RoleAuthorities",
                columns: new[] { "Id", "AuthLevel", "DeletedOn", "IsDefault", "Name" },
                values: new object[,]
                {
                    { 1, 0, null, false, "Admin" },
                    { 2, 50, null, true, "User" },
                    { 3, 100, null, true, "Guest" }
                });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedOn", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserRoleId" },
                values: new object[] { 1, 0, "d1d69d4a-e998-4b7a-a9e0-5d0e41eb61f5", null, "", true, false, null, "", "ROOT", "AQAAAAIAAYagAAAAEEXtt6AnxFl64Rbbe5CZG17RcWzd5wSMLDxcmm9zR6lelJA4kdyPr/xEImKRvKNvcg==", null, false, "6e6b1d35-71ad-44e4-ba5c-b0f2630cdd81", false, "Root", null });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "ActiveRole", "DeletedOn", "RoleAuthorityId", "UserAccountId" },
                values: new object[] { 1, true, null, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_NetworkedDeviceOwners_DeviceId",
                table: "NetworkedDeviceOwners",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserRoleId",
                table: "UserAccounts",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleAuthorityId",
                table: "UserRoles",
                column: "RoleAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserAccountId",
                table: "UserRoles",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkedDeviceOwners_UserAccounts_DeviceId",
                table: "NetworkedDeviceOwners",
                column: "DeviceId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_UserRoles_UserRoleId",
                table: "UserAccounts",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_UserAccounts_UserAccountId",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "NetworkedDeviceOwners");

            migrationBuilder.DropTable(
                name: "NetworkedDevices");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "RoleAuthorities");
        }
    }
}
