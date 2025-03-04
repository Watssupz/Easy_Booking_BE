using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class Add_Room_Media : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    media_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.media_id);
                    table.ForeignKey(
                        name: "FK_Media_Room_room_id",
                        column: x => x.room_id,
                        principalTable: "Room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "92514aca-328b-42e8-88fc-7314a05eae70");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "69b6d44c-8b3c-4fbd-81b2-92c4828302fe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "4b83830e-fed9-492b-819a-f7a2d0d552aa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "cb27fdc2-5e24-446b-a915-7ee427151b80");

            migrationBuilder.CreateIndex(
                name: "IX_Media_room_id",
                table: "Media",
                column: "room_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ee0d290a-5333-4645-a8d8-eb926b931483");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "54697565-96cc-40e0-acc6-767eab596754");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "72173081-fbb9-4d38-958b-3c90c5283df2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4",
                column: "ConcurrencyStamp",
                value: "038b1a86-20c6-42ca-8d6a-ddc3446efc86");
        }
    }
}
