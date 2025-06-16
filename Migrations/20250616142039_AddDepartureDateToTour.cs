using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LAPTRINHWEB.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartureDateToTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDate",
                table: "Tours",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureDate",
                table: "Tours");
        }
    }
}
