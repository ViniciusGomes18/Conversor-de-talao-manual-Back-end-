using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DSIN.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlateSnapshot = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    VehicleModelSnapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VehicleColorSnapshot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DriverNameSnapshot = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    DriverCpfSnapshot = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    ViolationCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ViolationDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketBooks_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketBooks_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketBooks_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Email",
                table: "Agents",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CPF",
                table: "Drivers",
                column: "CPF");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBooks_AgentId",
                table: "TicketBooks",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBooks_DriverId",
                table: "TicketBooks",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBooks_OccurredAt",
                table: "TicketBooks",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBooks_PlateSnapshot",
                table: "TicketBooks",
                column: "PlateSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBooks_VehicleId",
                table: "TicketBooks",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                table: "Vehicles",
                column: "Plate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketBooks");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
