using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LAPTRINHWEB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accommodation",
                columns: table => new
                {
                    ID_Accommodation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Star_Rating = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodation", x => x.ID_Accommodation);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID_Customer = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID_Customer);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID_Location = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID_Location);
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    ID_Tour = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Tour = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Start_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    End_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Max_Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tour", x => x.ID_Tour);
                });

            migrationBuilder.CreateTable(
                name: "TourGuide",
                columns: table => new
                {
                    ID_Guide = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Experience = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuide", x => x.ID_Guide);
                });

            migrationBuilder.CreateTable(
                name: "Transportation",
                columns: table => new
                {
                    ID_Transport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    License_Plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportation", x => x.ID_Transport);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    ID_Booking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Customer = table.Column<int>(type: "int", nullable: false),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Booking_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Number_Adults = table.Column<int>(type: "int", nullable: false),
                    Number_Children = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Total_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.ID_Booking);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_ID_Customer",
                        column: x => x.ID_Customer,
                        principalTable: "Customer",
                        principalColumn: "ID_Customer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itinerary",
                columns: table => new
                {
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itinerary", x => x.ID_Itinerary);
                    table.ForeignKey(
                        name: "FK_Itinerary_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourAccommodation",
                columns: table => new
                {
                    ID_TourAcc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    ID_Accommodation = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    Nights = table.Column<int>(type: "int", nullable: false),
                    Checkin_Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Checkout_Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourAccommodation", x => x.ID_TourAcc);
                    table.ForeignKey(
                        name: "FK_TourAccommodation_Accommodation_ID_Accommodation",
                        column: x => x.ID_Accommodation,
                        principalTable: "Accommodation",
                        principalColumn: "ID_Accommodation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourAccommodation_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourImage",
                columns: table => new
                {
                    ID_Image = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Image_URL = table.Column<string>(type: "text", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourImage", x => x.ID_Image);
                    table.ForeignKey(
                        name: "FK_TourImage_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourGuideAssignment",
                columns: table => new
                {
                    ID_Assignment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    ID_Guide = table.Column<int>(type: "int", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "date", nullable: false),
                    End_Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourGuideAssignment", x => x.ID_Assignment);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignment_TourGuide_ID_Guide",
                        column: x => x.ID_Guide,
                        principalTable: "TourGuide",
                        principalColumn: "ID_Guide",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourGuideAssignment_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourTransport",
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
                    Arrival_Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourTransport", x => x.ID_TourTransport);
                    table.ForeignKey(
                        name: "FK_TourTransport_Tour_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tour",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourTransport_Transportation_ID_Transport",
                        column: x => x.ID_Transport,
                        principalTable: "Transportation",
                        principalColumn: "ID_Transport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID_Payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Booking = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Paid_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID_Payment);
                    table.ForeignKey(
                        name: "FK_Payment_Booking_ID_Booking",
                        column: x => x.ID_Booking,
                        principalTable: "Booking",
                        principalColumn: "ID_Booking",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryLocation",
                columns: table => new
                {
                    ID_Item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false),
                    ID_Location = table.Column<int>(type: "int", nullable: false),
                    Visit_Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryLocation", x => x.ID_Item);
                    table.ForeignKey(
                        name: "FK_ItineraryLocation_Itinerary_ID_Itinerary",
                        column: x => x.ID_Itinerary,
                        principalTable: "Itinerary",
                        principalColumn: "ID_Itinerary",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItineraryLocation_Location_ID_Location",
                        column: x => x.ID_Location,
                        principalTable: "Location",
                        principalColumn: "ID_Location",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ID_Customer",
                table: "Booking",
                column: "ID_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ID_Tour",
                table: "Booking",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Itinerary_ID_Tour",
                table: "Itinerary",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocation_ID_Itinerary",
                table: "ItineraryLocation",
                column: "ID_Itinerary");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocation_ID_Location",
                table: "ItineraryLocation",
                column: "ID_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ID_Booking",
                table: "Payment",
                column: "ID_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodation_ID_Accommodation",
                table: "TourAccommodation",
                column: "ID_Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodation_ID_Tour",
                table: "TourAccommodation",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuide_Email",
                table: "TourGuide",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignment_ID_Guide",
                table: "TourGuideAssignment",
                column: "ID_Guide");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignment_ID_Tour",
                table: "TourGuideAssignment",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourImage_ID_Tour",
                table: "TourImage",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransport_ID_Tour",
                table: "TourTransport",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransport_ID_Transport",
                table: "TourTransport",
                column: "ID_Transport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItineraryLocation");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "TourAccommodation");

            migrationBuilder.DropTable(
                name: "TourGuideAssignment");

            migrationBuilder.DropTable(
                name: "TourImage");

            migrationBuilder.DropTable(
                name: "TourTransport");

            migrationBuilder.DropTable(
                name: "Itinerary");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Accommodation");

            migrationBuilder.DropTable(
                name: "TourGuide");

            migrationBuilder.DropTable(
                name: "Transportation");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Tour");
        }
    }
}
