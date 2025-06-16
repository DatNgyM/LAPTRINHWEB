using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LAPTRINHWEB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourGuideModel : Migration
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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
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
                name: "TourGuides",
                columns: table => new
                {
                    ID_Guide = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Experience = table.Column<string>(type: "ntext", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
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
                    Start_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    End_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Max_Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
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
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    License_Plate = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.ID_Transport);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID_Booking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Booking_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID_Booking);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Itineraries",
                columns: table => new
                {
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Day_Number = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
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
                    Checkout_Time = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourAccommodations", x => x.ID_TourAcc);
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
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_TourGuideAssignments_Tours_ID_Tour",
                        column: x => x.ID_Tour,
                        principalTable: "Tours",
                        principalColumn: "ID_Tour",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourImages",
                columns: table => new
                {
                    ID_Image = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Tour = table.Column<int>(type: "int", nullable: false),
                    Image_URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
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
                    Arrival_Time = table.Column<TimeSpan>(type: "time", nullable: true)
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
                        name: "FK_TourTransports_Transportations_ID_Transport",
                        column: x => x.ID_Transport,
                        principalTable: "Transportations",
                        principalColumn: "ID_Transport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID_Payment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Booking = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Paid_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payment_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
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

            migrationBuilder.CreateTable(
                name: "ItineraryDetails",
                columns: table => new
                {
                    ID_Detail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Itinerary = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activities = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItineraryDetails", x => x.ID_Detail);
                    table.ForeignKey(
                        name: "FK_ItineraryDetails_Itineraries_ID_Itinerary",
                        column: x => x.ID_Itinerary,
                        principalTable: "Itineraries",
                        principalColumn: "ID_Itinerary",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItineraryLocations",
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Booking_Date",
                table: "Bookings",
                column: "Booking_Date");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ID_Tour",
                table: "Bookings",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Status",
                table: "Bookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Itineraries_ID_Tour",
                table: "Itineraries",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryDetails_ID_Itinerary",
                table: "ItineraryDetails",
                column: "ID_Itinerary");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_ID_Itinerary",
                table: "ItineraryLocations",
                column: "ID_Itinerary");

            migrationBuilder.CreateIndex(
                name: "IX_ItineraryLocations_ID_Location",
                table: "ItineraryLocations",
                column: "ID_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ID_Booking",
                table: "Payments",
                column: "ID_Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Payment_Date",
                table: "Payments",
                column: "Payment_Date");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_ID_Accommodation",
                table: "TourAccommodations",
                column: "ID_Accommodation");

            migrationBuilder.CreateIndex(
                name: "IX_TourAccommodations_ID_Tour",
                table: "TourAccommodations",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_ID_Guide",
                table: "TourGuideAssignments",
                column: "ID_Guide");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_ID_Tour",
                table: "TourGuideAssignments",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuideAssignments_Start_Date_End_Date",
                table: "TourGuideAssignments",
                columns: new[] { "Start_Date", "End_Date" });

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
                name: "IX_Tours_Status",
                table: "Tours",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_ID_Tour",
                table: "TourTransports",
                column: "ID_Tour");

            migrationBuilder.CreateIndex(
                name: "IX_TourTransports_ID_Transport",
                table: "TourTransports",
                column: "ID_Transport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ItineraryDetails");

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
                name: "AspNetRoles");

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
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tours");
        }
    }
}
