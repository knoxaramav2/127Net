using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCore.Migrations
{
    /// <inheritdoc />
    public partial class device_connect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectedDevice",
                columns: table => new
                {
                    Device1Id = table.Column<int>(type: "int", nullable: false),
                    Device2Id = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedDevice", x => new { x.Device1Id, x.Device2Id });
                    table.ForeignKey(
                        name: "FK_ConnectedDevice_Devices_Device1Id",
                        column: x => x.Device1Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConnectedDevice_Devices_Device2Id",
                        column: x => x.Device2Id,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedDevice_Device2Id",
                table: "ConnectedDevice",
                column: "Device2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectedDevice");
        }
    }
}
