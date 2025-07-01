using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OneTierShallBeAbleToHaveManyPricings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pricings_TierId",
                table: "Pricings");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_TierId",
                table: "Pricings",
                column: "TierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pricings_TierId",
                table: "Pricings");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_TierId",
                table: "Pricings",
                column: "TierId",
                unique: true);
        }
    }
}
