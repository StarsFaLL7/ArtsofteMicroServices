using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCreatureInSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHealth",
                table: "CreatureInSessions");

            migrationBuilder.DropColumn(
                name: "CurrentArmor",
                table: "CharacterInSessions");

            migrationBuilder.DropColumn(
                name: "CurrentHealth",
                table: "CharacterInSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentHealth",
                table: "CreatureInSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentArmor",
                table: "CharacterInSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentHealth",
                table: "CharacterInSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
