using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSIN.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketImageBase64ToTicketBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketImageBase64",
                table: "TicketBooks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketImageBase64",
                table: "TicketBooks");
        }
    }
}
