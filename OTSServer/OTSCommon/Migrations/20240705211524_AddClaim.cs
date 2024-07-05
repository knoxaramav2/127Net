using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OTSCommon.Migrations
{
    /// <inheritdoc />
    public partial class AddClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserIdentityClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentityClaim", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a06520f7-85dc-46c3-a139-d316cfc54c88", "AQAAAAIAAYagAAAAEFsTpdmaz4zyTwpX4BkM/qBejQ9ZnyhsQixvGLGFco3+Rtg7INx0Zs6nUzg9XUit2w==", "0887161c-0dfe-416e-b881-3aeda5508419" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIdentityClaim");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1d69d4a-e998-4b7a-a9e0-5d0e41eb61f5", "AQAAAAIAAYagAAAAEEXtt6AnxFl64Rbbe5CZG17RcWzd5wSMLDxcmm9zR6lelJA4kdyPr/xEImKRvKNvcg==", "6e6b1d35-71ad-44e4-ba5c-b0f2630cdd81" });
        }
    }
}
