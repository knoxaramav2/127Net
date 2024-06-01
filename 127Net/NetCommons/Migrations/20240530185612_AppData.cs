using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCommons.Migrations
{
    /// <inheritdoc />
    public partial class AppData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NetControls_NetComponents_NetComponentId",
                table: "NetControls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetControls",
                table: "NetControls");

            migrationBuilder.DropColumn(
                name: "ComponentLibId",
                table: "NetControls");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "NetControls");

            migrationBuilder.RenameTable(
                name: "NetControls",
                newName: "NetControl");

            migrationBuilder.RenameIndex(
                name: "IX_NetControls_NetComponentId",
                table: "NetControl",
                newName: "IX_NetControl_NetComponentId");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "NetComponents",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ComponentName",
                table: "NetComponents",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "AppData",
                table: "NetComponents",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "NetComponents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "OS",
                table: "Devices",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Devices",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HwId",
                table: "Devices",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Devices",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Devices",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "AuthRoles",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "ComponentLibName",
                table: "NetControl",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FunctionName",
                table: "NetControl",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetControl",
                table: "NetControl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetControl_NetComponents_NetComponentId",
                table: "NetControl",
                column: "NetComponentId",
                principalTable: "NetComponents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NetControl_NetComponents_NetComponentId",
                table: "NetControl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetControl",
                table: "NetControl");

            migrationBuilder.DropColumn(
                name: "AppData",
                table: "NetComponents");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "NetComponents");

            migrationBuilder.DropColumn(
                name: "ComponentLibName",
                table: "NetControl");

            migrationBuilder.DropColumn(
                name: "FunctionName",
                table: "NetControl");

            migrationBuilder.RenameTable(
                name: "NetControl",
                newName: "NetControls");

            migrationBuilder.RenameIndex(
                name: "IX_NetControl_NetComponentId",
                table: "NetControls",
                newName: "IX_NetControls_NetComponentId");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "NetComponents",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "ComponentName",
                table: "NetComponents",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "OS",
                table: "Devices",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Devices",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HwId",
                table: "Devices",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "FriendlyName",
                table: "Devices",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Devices",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "RoleName",
                table: "AuthRoles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "ComponentLibId",
                table: "NetControls",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "FunctionId",
                table: "NetControls",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetControls",
                table: "NetControls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetControls_NetComponents_NetComponentId",
                table: "NetControls",
                column: "NetComponentId",
                principalTable: "NetComponents",
                principalColumn: "Id");
        }
    }
}
