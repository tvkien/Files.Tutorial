using Microsoft.EntityFrameworkCore.Migrations;

namespace Files.Tutorial.Migrations
{
    public partial class AddColumn_Size : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "FileOnFileSystem",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "FileOnDatabase",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileOnFileSystem");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "FileOnDatabase");
        }
    }
}
