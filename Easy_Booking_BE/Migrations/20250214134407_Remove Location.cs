using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class RemoveLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Location_location_id",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_location_id",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "Room");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "location_id",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Room_location_id",
                table: "Room",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Location_location_id",
                table: "Room",
                column: "location_id",
                principalTable: "Location",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
