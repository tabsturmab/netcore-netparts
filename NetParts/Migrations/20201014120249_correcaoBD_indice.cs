using Microsoft.EntityFrameworkCore.Migrations;

namespace NetParts.Migrations
{
    public partial class correcaoBD_indice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Collaborators",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Collaborators",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Collaborators",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Collaborators",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "NameCategory",
                table: "Categories",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Address",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Address",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.CreateIndex(
                name: "Index_Product_PartNumber",
                table: "Products",
                column: "PartNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Collaborator_Email",
                table: "Collaborators",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Address_ZipCode",
                table: "Address",
                column: "ZipCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Product_PartNumber",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "Index_Collaborator_Email",
                table: "Collaborators");

            migrationBuilder.DropIndex(
                name: "Index_Address_ZipCode",
                table: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "PartNumber",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Collaborators",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Collaborators",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Collaborators",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Collaborators",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "NameCategory",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "Address",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Address",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }
    }
}
