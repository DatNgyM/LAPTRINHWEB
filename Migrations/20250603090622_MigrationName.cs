using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LAPTRINHWEB.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    ID_Accommodation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    Star_Rating = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.ID_Accommodation);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID_Location = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID_Location);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID_Role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID_Role);
                });

            migrationBuilder.CreateTable(
                name: "TourGuides",
                columns: table => new
                {
                    ID_Guide = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 100, nullable: false),
                    Experience = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuides", x => x.ID_Guide);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    ID_Tour = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Tour = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Start_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 200, nullable: false),
                    End_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Max_Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.ID_Tour);
                });

            migrationBuilder.CreateTable(
                name: "Transportations",
                columns: table => new
                {
                    ID_Transport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 100, nullable: true),
                    License_Plate = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.ID_Transport);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID_User = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Full_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password_Hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Date_Of_Birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ID_Role = table.Column<int>(type: "int", nullable: false),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Last_Login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Is_Active = table.Column<bool>(type: "bit", nullable: false),
                    Email_Verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID_User);
                    table.ForeignKey(
                        name: "FK_Users_Roles_ID_Role",
                        column: x => x.ID_Role,
                        principalTable: "Roles",
                        principalColumn: "ID_Role",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    TourID_Tour = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itineraries", x => x.ID_Itinerary);
                    table.ForeignKey(
                        name: "FK_Itineraries_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Itineraries_Tours_TourID_Tour",
                        column: x => x.TourID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour");
                });

            migrationBuilder.CreateTable(
                name: "TourAccommodations",
                columns: table => new
                {
                    ID_TourAcc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    ID_Accommodation = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    Checkin_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Checkout_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    AccommodationID_Accommodation = table.Column<int>(type: "int", nullable: true),
                    TourID_Tour = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourAccommodations", x => x.ID_TourAcc);
                    table.ForeignKey(
                        name: "FK_TourAccommodations_Accommodations_AccommodationID_Accommodation",
                        column: x => x.AccommodationID_Accommodation,
                        principalTable: "Accommodations",
                        principalColumn: "ID_Accommodation");
                    table.ForeignKey(
                        name: "FK_TourAccommodations_Accommodations_ID_Accommodation",
                        column: x => x.ID_Accommodation,
                        principalTable: "Accommodations",
                        principalColumn: "ID_Accommodation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourAccommodations_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourAccommodations_Tours_TourID_Tour",
                        column: x => x.TourID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour");
                });

            migrationBuilder.CreateTable(
                name: "TourGuideAssignments",
                columns: table => new
                {
                    ID_Assignment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    ID_Guide = table.Column<int>(type: "int", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TourGuideID_Guide = table.Column<int>(type: "int", nullable: true),
                    TourID_Tour = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuideAssignments", x => x.ID_Assignment);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignments_TourGuides_ID_Guide",
                        column: x => x.ID_Guide,
                        principalTable: "TourGuides",
                        principalColumn: "ID_Guide",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignments_TourGuides_TourGuideID_Guide",
                        column: x => x.TourGuideID_Guide,
                        principalTable: "TourGuides",
                        principalColumn: "ID_Guide");
                    table.ForeignKey(
                        name: "FK_TourGuideAssignments_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignments_Tours_TourID_Tour",
                        column: x => x.TourID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour");
                });

            migrationBuilder.CreateTable(
                name: "TourImages",
                columns: table => new
                {
                    ID_Image = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Image_URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourImages", x => x.ID_Image);
                    table.ForeignKey(
                        name: "FK_TourImages_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTransports",
                columns: table => new
                {
                    ID_TourTransport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    ID_Transport = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    From_Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    To_Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Departure_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Arrival_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    TourID_Tour = table.Column<int>(type: "int", nullable: true),
                    TransportationID_Transport = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTransports", x => x.ID_TourTransport);
                    table.ForeignKey(
                        name: "FK_TourTransports_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTransports_Tours_TourID_Tour",
                        column: x => x.TourID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour");
                    table.ForeignKey(
                        name: "FK_TourTransports_Transportations_ID_Transport",
                        column: x => x.ID_Transport,
                        principalTable: "Transportations",
                        principalColumn: "ID_Transport",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTransports_Transportations_TransportationID_Transport",
                        column: x => x.TransportationID_Transport,
                        principalTable: "Transportations",
                        principalColumn: "ID_Transport");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID_Booking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Customer = table.Column<int>(type: "int", nullable: false),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Booking_Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Number_Adults = table.Column<int>(type: "int", nullable: false),
                    Number_Children = table.Column<int>(type: "int", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "ntext", nullable: true),
                    ID_User = table.Column<int>(type: "int", nullable: true),
                    TourID_Tour = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID_Booking);
                    table.ForeignKey(
                        name: "FK_Bookings_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Tours_TourID_Tour",
                        column: x => x.TourID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour");
                    table.ForeignKey(
                        name: "FK_Bookings_Users_ID_Customer",
                        column: x => x.ID_Customer,
                        principalTable: "Users",
                        principalColumn: "ID_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryLocations",
                columns: table => new
                {
                    ID_Item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false),
                    ID_Location = table.Column<int>(type: "int", nullable: false),
                    Visit_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    LocationID_Location = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryLocations", x => x.ID_Item);
                    table.ForeignKey(
                        name: "FK_ItineraryLocations_Itineraries_ID_Itinerary",
                        column: x => x.ID_Itinerary,
                        principalTable: "Itineraries",
                        principalColumn: "ID_Itinerary",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryLocations_Locations_ID_Location",
                        column: x => x.ID_Location,
                        principalTable: "Locations",
                        principalColumn: "ID_Location",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryLocations_Locations_LocationID_Location",
                        column: x => x.LocationID_Location,
                        principalTable: "Locations",
                        principalColumn: "ID_Location");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID_Payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Booking = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Paid_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment_Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID_Payment);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_ID_Booking",
                        column: x => x.ID_Booking,
                        principalTable: "Bookings",
                        principalColumn: "ID_Booking",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Booking_Date",
                table: "Bookings",
                column: "Booking_Date");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ID_Customer",
                table: "Bookings",
                column: "ID_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ID_Tour",
                table: "Bookings",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TourID_Tour",
                table: "Bookings",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_ID_Tour",
                table: "Itineraries",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_TourID_Tour",
                table: "Itineraries",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_ID_Itinerary",
                table: "ItineraryLocations",
                column: "ID_Itinerary");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_ID_Location",
                table: "ItineraryLocations",
                column: "ID_Location");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_LocationID_Location",
                table: "ItineraryLocations",
                column: "LocationID_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ID_Booking",
                table: "Payments",
                column: "ID_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Role_Name",
                table: "Roles",
                column: "Role_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_AccommodationID_Accommodation",
                table: "TourAccommodations",
                column: "AccommodationID_Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_ID_Accommodation",
                table: "TourAccommodations",
                column: "ID_Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_ID_Tour",
                table: "TourAccommodations",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_TourID_Tour",
                table: "TourAccommodations",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_ID_Guide",
                table: "TourGuideAssignments",
                column: "ID_Guide");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_ID_Tour",
                table: "TourGuideAssignments",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_TourGuideID_Guide",
                table: "TourGuideAssignments",
                column: "TourGuideID_Guide");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_TourID_Tour",
                table: "TourGuideAssignments",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuides_Email",
                table: "TourGuides",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourImages_ID_Tour",
                table: "TourImages",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Name_Tour",
                table: "Tours",
                column: "Name_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_ID_Tour",
                table: "TourTransports",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_ID_Transport",
                table: "TourTransports",
                column: "ID_Transport");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_TourID_Tour",
                table: "TourTransports",
                column: "TourID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_TransportationID_Transport",
                table: "TourTransports",
                column: "TransportationID_Transport");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ID_Role",
                table: "Users",
                column: "ID_Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryLocations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TourAccommodations");

            migrationBuilder.DropTable(
                name: "TourGuideAssignments");

            migrationBuilder.DropTable(
                name: "TourImages");

            migrationBuilder.DropTable(
                name: "TourTransports");

            migrationBuilder.DropTable(
                name: "Itineraries");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "TourGuides");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
