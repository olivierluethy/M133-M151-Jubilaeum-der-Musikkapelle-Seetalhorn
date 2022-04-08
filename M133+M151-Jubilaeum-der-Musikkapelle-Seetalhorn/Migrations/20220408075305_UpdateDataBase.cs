using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Migrations
{
    public partial class UpdateDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Antwort1",
                table: "Seetalhorn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Antwort2",
                table: "Seetalhorn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Antwort3",
                table: "Seetalhorn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Antwort4",
                table: "Seetalhorn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Antwort5",
                table: "Seetalhorn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Antwort1",
                table: "Seetalhorn");

            migrationBuilder.DropColumn(
                name: "Antwort2",
                table: "Seetalhorn");

            migrationBuilder.DropColumn(
                name: "Antwort3",
                table: "Seetalhorn");

            migrationBuilder.DropColumn(
                name: "Antwort4",
                table: "Seetalhorn");

            migrationBuilder.DropColumn(
                name: "Antwort5",
                table: "Seetalhorn");
        }
    }
}
