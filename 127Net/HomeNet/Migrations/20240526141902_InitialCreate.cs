using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace HomeNet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "longtext", nullable: false),
                    AuthLevel = table.Column<int>(type: "int", nullable: false),
                    ForceCredential = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReauthTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DowngradeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Roles_DowngradeId",
                        column: x => x.DowngradeId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NetComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ComponentName = table.Column<string>(type: "longtext", nullable: false),
                    Version = table.Column<string>(type: "longtext", nullable: false),
                    MinimumRoleAuthorityId = table.Column<int>(type: "int", nullable: false),
                    FirstReleaseTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetComponents_Roles_MinimumRoleAuthorityId",
                        column: x => x.MinimumRoleAuthorityId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    RoleAuthorityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleAuthorityId",
                        column: x => x.RoleAuthorityId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NetControls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ComponentLibId = table.Column<string>(type: "longtext", nullable: false),
                    FunctionId = table.Column<string>(type: "longtext", nullable: false),
                    NetComponentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetControls_NetComponents_NetComponentId",
                        column: x => x.NetComponentId,
                        principalTable: "NetComponents",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NetListeners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NetComponentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetListeners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetListeners_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetListeners_NetComponents_NetComponentId",
                        column: x => x.NetComponentId,
                        principalTable: "NetComponents",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NetComponents_MinimumRoleAuthorityId",
                table: "NetComponents",
                column: "MinimumRoleAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_NetControls_NetComponentId",
                table: "NetControls",
                column: "NetComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_NetListeners_DeviceId",
                table: "NetListeners",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_NetListeners_NetComponentId",
                table: "NetListeners",
                column: "NetComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DowngradeId",
                table: "Roles",
                column: "DowngradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeviceId",
                table: "Users",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleAuthorityId",
                table: "Users",
                column: "RoleAuthorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetControls");

            migrationBuilder.DropTable(
                name: "NetListeners");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "NetComponents");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
