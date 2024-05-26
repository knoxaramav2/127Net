using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCore.Migrations
{
    /// <inheritdoc />
    public partial class updateRoleAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthLevel",
                table: "AuthRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DowngradeId",
                table: "AuthRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForceCredential",
                table: "AuthRoles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReauthTime",
                table: "AuthRoles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "AuthRoles",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_AuthRoles_DowngradeId",
                table: "AuthRoles",
                column: "DowngradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthRoles_AuthRoles_DowngradeId",
                table: "AuthRoles",
                column: "DowngradeId",
                principalTable: "AuthRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthRoles_AuthRoles_DowngradeId",
                table: "AuthRoles");

            migrationBuilder.DropIndex(
                name: "IX_AuthRoles_DowngradeId",
                table: "AuthRoles");

            migrationBuilder.DropColumn(
                name: "AuthLevel",
                table: "AuthRoles");

            migrationBuilder.DropColumn(
                name: "DowngradeId",
                table: "AuthRoles");

            migrationBuilder.DropColumn(
                name: "ForceCredential",
                table: "AuthRoles");

            migrationBuilder.DropColumn(
                name: "ReauthTime",
                table: "AuthRoles");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "AuthRoles");
        }
    }
}
