using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace HomeCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleAuthorityId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuthRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleAuthorityId",
                table: "AspNetUsers",
                column: "RoleAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AuthRoles_RoleAuthorityId",
                table: "AspNetUsers",
                column: "RoleAuthorityId",
                principalTable: "AuthRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AuthRoles_RoleAuthorityId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AuthRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleAuthorityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleAuthorityId",
                table: "AspNetUsers");
        }
    }
}
