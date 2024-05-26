using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeNet.Migrations
{
    /// <inheritdoc />
    public partial class OpAuth_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleAuthorityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleAuthorityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleAuthorityId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "OperatingAuthorityId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OperatingAuthorityId",
                table: "Users",
                column: "OperatingAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_OperatingAuthorityId",
                table: "Users",
                column: "OperatingAuthorityId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_OperatingAuthorityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OperatingAuthorityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OperatingAuthorityId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RoleAuthorityId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleAuthorityId",
                table: "Users",
                column: "RoleAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleAuthorityId",
                table: "Users",
                column: "RoleAuthorityId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
