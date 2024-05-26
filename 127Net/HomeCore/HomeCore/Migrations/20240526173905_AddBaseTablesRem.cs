using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace HomeCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseTablesRem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DisplayName = table.Column<string>(type: "longtext", nullable: false),
                    HwId = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
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
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    MinimumRoleAuthorityId = table.Column<int>(type: "int", nullable: false),
                    FirstReleaseTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetComponents_AuthRoles_MinimumRoleAuthorityId",
                        column: x => x.MinimumRoleAuthorityId,
                        principalTable: "AuthRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AspNetUsers_DeviceId",
                table: "AspNetUsers",
                column: "DeviceId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Devices_DeviceId",
                table: "AspNetUsers",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Devices_DeviceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "NetControls");

            migrationBuilder.DropTable(
                name: "NetListeners");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "NetComponents");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DeviceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "AspNetUsers");
        }
    }
}
