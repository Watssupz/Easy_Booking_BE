using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class removeFloor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Floor_floor_id",
                table: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Room_floor_id",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "floor_id",
                table: "Room");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "floor_id",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Room_floor_id",
                table: "Room",
                column: "floor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Floor_floor_id",
                table: "Room",
                column: "floor_id",
                principalTable: "Floor",
                principalColumn: "floor_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
