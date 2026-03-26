using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookGate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: "COMPLETED",
                column: "StatusName",
                value: "Hoàn thành");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: "PENDING",
                column: "StatusName",
                value: "Chờ xác nhận");

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[] { "AWAITING_SHIPMENT", "Chờ giao hàng" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: "AWAITING_SHIPMENT");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: "COMPLETED",
                column: "StatusName",
                value: "Đã hoàn thành");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "StatusId",
                keyValue: "PENDING",
                column: "StatusName",
                value: "Chờ xử lý");
        }
    }
}
