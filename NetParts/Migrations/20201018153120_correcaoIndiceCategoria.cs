using Microsoft.EntityFrameworkCore.Migrations;

namespace NetParts.Migrations
{
    public partial class correcaoIndiceCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Address_ZipCode",
                table: "Address");

            migrationBuilder.CreateIndex(
                name: "Index_Category_NameCategory",
                table: "Categories",
                column: "NameCategory",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Address_ZipCode",
                table: "Address",
                column: "ZipCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Category_NameCategory",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "Index_Address_ZipCode",
                table: "Address");

            migrationBuilder.CreateIndex(
                name: "Index_Address_ZipCode",
                table: "Address",
                column: "ZipCode",
                unique: true);
        }
    }
}
