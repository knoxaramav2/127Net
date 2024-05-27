using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCore.Migrations
{
    /// <inheritdoc />
    public partial class device_info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "Devices",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Devices",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "Devices",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "Devices");
        }
    }
}
