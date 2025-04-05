using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedScreeningIdToBookingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScreeningId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ScreeningId",
                table: "Bookings",
                column: "ScreeningId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Screenings_ScreeningId",
                table: "Bookings",
                column: "ScreeningId",
                principalTable: "Screenings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Screenings_ScreeningId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ScreeningId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ScreeningId",
                table: "Bookings");
        }
    }
}
