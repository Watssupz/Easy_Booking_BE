using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class UpdateRoomstatusfieldstype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "booking_status_name",
                table: "Booking_Status",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "booking_status_name",
                table: "Booking_Status",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
