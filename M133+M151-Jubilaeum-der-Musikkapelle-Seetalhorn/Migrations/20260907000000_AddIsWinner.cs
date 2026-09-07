using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Migrations
{
    public partial class AddIsWinner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWinner",
                table: "Seetalhorn",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWinner",
                table: "Seetalhorn");
        }
    }
}
