using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class DB_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking_Status",
                columns: table => new
                {
                    booking_status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_status_name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking_Status", x => x.booking_status_id);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    feature_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    feature_name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.feature_id);
                });

            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    floor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    floor_name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.floor_id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    location_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.location_id);
                });

            migrationBuilder.CreateTable(
                name: "Payment_Status",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_status_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_Status", x => x.payment_id);
                });

            migrationBuilder.CreateTable(
                name: "Room_Status",
                columns: table => new
                {
                    room_status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_status_name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room_Status", x => x.room_status_id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start_date_booking = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date_booking = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check_in = table.Column<DateTime>(type: "datetime2", nullable: false),
                    check_out = table.Column<DateTime>(type: "datetime2", nullable: false),
                    num_adults = table.Column<int>(type: "int", nullable: false),
                    num_children = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    booking_status = table.Column<int>(type: "int", nullable: false),
                    payment_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Booking_Booking_Status_booking_status",
                        column: x => x.booking_status,
                        principalTable: "Booking_Status",
                        principalColumn: "booking_status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Payment_Status_payment_status",
                        column: x => x.payment_status,
                        principalTable: "Payment_Status",
                        principalColumn: "payment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price_per_night = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    floor_id = table.Column<int>(type: "int", nullable: false),
                    room_status_id = table.Column<int>(type: "int", nullable: false),
                    location_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.room_id);
                    table.ForeignKey(
                        name: "FK_Room_Floor_floor_id",
                        column: x => x.floor_id,
                        principalTable: "Floor",
                        principalColumn: "floor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_Location_location_id",
                        column: x => x.location_id,
                        principalTable: "Location",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_Room_Status_room_status_id",
                        column: x => x.room_status_id,
                        principalTable: "Room_Status",
                        principalColumn: "room_status_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking_Room",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking_Room", x => new { x.booking_id, x.room_id });
                    table.ForeignKey(
                        name: "FK_Booking_Room_Booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "booking_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Room_Room_room_id",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room_Feature",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "int", nullable: false),
                    feature_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room_Feature", x => new { x.room_id, x.feature_id });
                    table.ForeignKey(
                        name: "FK_Room_Feature_Feature_feature_id",
                        column: x => x.feature_id,
                        principalTable: "Feature",
                        principalColumn: "feature_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_Feature_Room_room_id",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_booking_status",
                table: "Booking",
                column: "booking_status");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_payment_status",
                table: "Booking",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Room_room_id",
                table: "Booking_Room",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_floor_id",
                table: "Room",
                column: "floor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_location_id",
                table: "Room",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_room_status_id",
                table: "Room",
                column: "room_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Feature_feature_id",
                table: "Room_Feature",
                column: "feature_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking_Room");

            migrationBuilder.DropTable(
                name: "Room_Feature");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Booking_Status");

            migrationBuilder.DropTable(
                name: "Payment_Status");

            migrationBuilder.DropTable(
                name: "Floor");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Room_Status");
        }
    }
}
