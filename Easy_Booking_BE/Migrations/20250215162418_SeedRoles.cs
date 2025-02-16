using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Easy_Booking_BE.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "feeaa0e6-a083-481e-9746-c9ac014432af", "Administrator", "ADMINISTRATOR" },
                    { "2", "af05418c-c709-40bc-9deb-e31c3532d36c", "Customer", "CUSTOMER" },
                    { "3", "71a7af82-f37e-4aa7-b59a-6b8361dac396", "Host", "HOST" },
                    { "4", "fbf64db5-7217-4c1b-a85d-7ef486c5757b", "Support Staff", "SUPPORT STAFF" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4");
        }
    }
}
