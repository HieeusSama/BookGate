using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookGate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat1e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "TheLoai",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TenSach",
                table: "Books",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "TacGia",
                table: "Books",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "NgayXuatBan",
                table: "Books",
                newName: "PublicationDate");

            migrationBuilder.RenameColumn(
                name: "MaSach",
                table: "Books",
                newName: "PublisherId");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Books",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "BookId");

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublisherName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "TheLoai");

            migrationBuilder.RenameColumn(
                name: "PublisherId",
                table: "Books",
                newName: "MaSach");

            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "Books",
                newName: "NgayXuatBan");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "TenSach");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Books",
                newName: "TacGia");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "MaSach");
        }
    }
}
