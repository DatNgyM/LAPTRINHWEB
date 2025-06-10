using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LAPTRINHWEB.Migrations
{
    /// <inheritdoc />
    public partial class CompleteDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Tours_TourID_Tour",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Itineraries_Tours_TourID_Tour",
                table: "Itineraries");

            migrationBuilder.DropForeignKey(
                name: "FK_ItineraryLocations_Locations_LocationID_Location",
                table: "ItineraryLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourAccommodations_Accommodations_AccommodationID_Accommodation",
                table: "TourAccommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourAccommodations_Tours_TourID_Tour",
                table: "TourAccommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_TourGuideAssignments_TourGuides_TourGuideID_Guide",
                table: "TourGuideAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TourGuideAssignments_Tours_TourID_Tour",
                table: "TourGuideAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTransports_Tours_TourID_Tour",
                table: "TourTransports");

            migrationBuilder.DropForeignKey(
                name: "FK_TourTransports_Transportations_TransportationID_Transport",
                table: "TourTransports");

            migrationBuilder.DropIndex(
                name: "IX_TourTransports_TourID_Tour",
                table: "TourTransports");

            migrationBuilder.DropIndex(
                name: "IX_TourTransports_TransportationID_Transport",
                table: "TourTransports");

            migrationBuilder.DropIndex(
                name: "IX_TourGuideAssignments_TourGuideID_Guide",
                table: "TourGuideAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TourGuideAssignments_TourID_Tour",
                table: "TourGuideAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TourAccommodations_AccommodationID_Accommodation",
                table: "TourAccommodations");

            migrationBuilder.DropIndex(
                name: "IX_TourAccommodations_TourID_Tour",
                table: "TourAccommodations");

            migrationBuilder.DropIndex(
                name: "IX_ItineraryLocations_LocationID_Location",
                table: "ItineraryLocations");

            migrationBuilder.DropIndex(
                name: "IX_Itineraries_TourID_Tour",
                table: "Itineraries");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TourID_Tour",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TourID_Tour",
                table: "TourTransports");

            migrationBuilder.DropColumn(
                name: "TransportationID_Transport",
                table: "TourTransports");

            migrationBuilder.DropColumn(
                name: "TourGuideID_Guide",
                table: "TourGuideAssignments");

            migrationBuilder.DropColumn(
                name: "TourID_Tour",
                table: "TourGuideAssignments");

            migrationBuilder.DropColumn(
                name: "AccommodationID_Accommodation",
                table: "TourAccommodations");

            migrationBuilder.DropColumn(
                name: "TourID_Tour",
                table: "TourAccommodations");

            migrationBuilder.DropColumn(
                name: "LocationID_Location",
                table: "ItineraryLocations");

            migrationBuilder.DropColumn(
                name: "TourID_Tour",
                table: "Itineraries");

            migrationBuilder.DropColumn(
                name: "TourID_Tour",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Tours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Payment_Date",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Number_Children",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Booking_Date",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Status",
                table: "Tours",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_Start_Date_End_Date",
                table: "TourGuideAssignments",
                columns: new[] { "Start_Date", "End_Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Payment_Date",
                table: "Payments",
                column: "Payment_Date");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Status",
                table: "Bookings",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tours_Status",
                table: "Tours");

            migrationBuilder.DropIndex(
                name: "IX_TourGuideAssignments_Start_Date_End_Date",
                table: "TourGuideAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Payment_Date",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_Status",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "TourID_Tour",
                table: "TourTransports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportationID_Transport",
                table: "TourTransports",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Tours",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TourGuideID_Guide",
                table: "TourGuideAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TourID_Tour",
                table: "TourGuideAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccommodationID_Accommodation",
                table: "TourAccommodations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TourID_Tour",
                table: "TourAccommodations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Payment_Date",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LocationID_Location",
                table: "ItineraryLocations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TourID_Tour",
                table: "Itineraries",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Number_Children",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Booking_Date",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TourID_Tour",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_TourID_Tour",
                table: "TourTransports",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_TransportationID_Transport",
                table: "TourTransports",
                column: "TransportationID_Transport");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_TourGuideID_Guide",
                table: "TourGuideAssignments",
                column: "TourGuideID_Guide");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_TourID_Tour",
                table: "TourGuideAssignments",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_AccommodationID_Accommodation",
                table: "TourAccommodations",
                column: "AccommodationID_Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_TourID_Tour",
                table: "TourAccommodations",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_LocationID_Location",
                table: "ItineraryLocations",
                column: "LocationID_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_TourID_Tour",
                table: "Itineraries",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourID_Tour",
                table: "Bookings",
                column: "TourID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Tours_TourID_Tour",
                table: "Bookings",
                column: "TourID_Tour",
                principalTable: "Tours",
                principalColumn: "ID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_Itineraries_Tours_TourID_Tour",
                table: "Itineraries",
                column: "TourID_Tour",
                principalTable: "Tours",
                principalColumn: "ID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_ItineraryLocations_Locations_LocationID_Location",
                table: "ItineraryLocations",
                column: "LocationID_Location",
                principalTable: "Locations",
                principalColumn: "ID_Location");

            migrationBuilder.AddForeignKey(
                name: "FK_TourAccommodations_Accommodations_AccommodationID_Accommodation",
                table: "TourAccommodations",
                column: "AccommodationID_Accommodation",
                principalTable: "Accommodations",
                principalColumn: "ID_Accommodation");

            migrationBuilder.AddForeignKey(
                name: "FK_TourAccommodations_Tours_TourID_Tour",
                table: "TourAccommodations",
                column: "TourID_Tour",
                principalTable: "Tours",
                principalColumn: "ID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_TourGuideAssignments_TourGuides_TourGuideID_Guide",
                table: "TourGuideAssignments",
                column: "TourGuideID_Guide",
                principalTable: "TourGuides",
                principalColumn: "ID_Guide");

            migrationBuilder.AddForeignKey(
                name: "FK_TourGuideAssignments_Tours_TourID_Tour",
                table: "TourGuideAssignments",
                column: "TourID_Tour",
                principalTable: "Tours",
                principalColumn: "ID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTransports_Tours_TourID_Tour",
                table: "TourTransports",
                column: "TourID_Tour",
                principalTable: "Tours",
                principalColumn: "ID_Tour");

            migrationBuilder.AddForeignKey(
                name: "FK_TourTransports_Transportations_TransportationID_Transport",
                table: "TourTransports",
                column: "TransportationID_Transport",
                principalTable: "Transportations",
                principalColumn: "ID_Transport");
        }
    }
}
