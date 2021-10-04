using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetParts.Migrations
{
    public partial class CriacaoDBCorrecao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IdCategory = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameCategory = table.Column<string>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    CategoryMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IdCategory);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_CategoryMasterId",
                        column: x => x.CategoryMasterId,
                        principalTable: "Categories",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: true),
                    LogLevel = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    IdManufacturer = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameManufacturer = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.IdManufacturer);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalAssistance",
                columns: table => new
                {
                    IdTecAssistance = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocialReason = table.Column<string>(nullable: false),
                    Cnpj = table.Column<string>(nullable: false),
                    StateInscription = table.Column<string>(maxLength: 14, nullable: false),
                    EmailAta = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    DateRegister = table.Column<DateTime>(nullable: false),
                    EnabledDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalAssistance", x => x.IdTecAssistance);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    IdProduct = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PartNumber = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    Width1 = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    IdCategory = table.Column<int>(nullable: false),
                    IdManufacturer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.IdProduct);
                    table.ForeignKey(
                        name: "FK_Products_Categories_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "Categories",
                        principalColumn: "IdCategory",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Manufacturers_IdManufacturer",
                        column: x => x.IdManufacturer,
                        principalTable: "Manufacturers",
                        principalColumn: "IdManufacturer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    IdAddress = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address1 = table.Column<string>(maxLength: 100, nullable: false),
                    NumberAta = table.Column<string>(maxLength: 10, nullable: false),
                    Complement = table.Column<string>(maxLength: 100, nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    District = table.Column<string>(maxLength: 500, nullable: false),
                    City = table.Column<string>(maxLength: 60, nullable: false),
                    State1 = table.Column<string>(maxLength: 2, nullable: false),
                    IdTecAssistance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.IdAddress);
                    table.ForeignKey(
                        name: "FK_Address_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Archives",
                columns: table => new
                {
                    IdArchive = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Way = table.Column<string>(nullable: true),
                    IdTecAssistance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archives", x => x.IdArchive);
                    table.ForeignKey(
                        name: "FK_Archives_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    IdCollaborator = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Cpf = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    TypeCollaborator = table.Column<string>(nullable: false),
                    IdTecAssistance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.IdCollaborator);
                    table.ForeignKey(
                        name: "FK_Collaborators_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdOrder = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<string>(nullable: true),
                    FreightCompany = table.Column<string>(nullable: true),
                    FreightCodTracking = table.Column<string>(nullable: true),
                    FormPayment = table.Column<string>(nullable: true),
                    ValueTotal = table.Column<decimal>(nullable: false),
                    DataTransaction = table.Column<string>(nullable: true),
                    DataProducts = table.Column<string>(nullable: true),
                    DateRegisterOrder = table.Column<DateTime>(nullable: false),
                    Situation = table.Column<string>(nullable: true),
                    NFe = table.Column<string>(nullable: true),
                    IdTecAssistance = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder);
                    table.ForeignKey(
                        name: "FK_Orders_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalAssistanceManufacturer",
                columns: table => new
                {
                    IdTecAssistance = table.Column<int>(nullable: false),
                    IdManufacturer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalAssistanceManufacturer", x => new { x.IdTecAssistance, x.IdManufacturer });
                    table.ForeignKey(
                        name: "FK_TechnicalAssistanceManufacturer_Manufacturers_IdManufacturer",
                        column: x => x.IdManufacturer,
                        principalTable: "Manufacturers",
                        principalColumn: "IdManufacturer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalAssistanceManufacturer_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisement",
                columns: table => new
                {
                    IdAdvert = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    IdTecAssistance = table.Column<int>(nullable: false),
                    IdProduct = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.IdAdvert);
                    table.ForeignKey(
                        name: "FK_Advertisement_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_TechnicalAssistance_IdTecAssistance",
                        column: x => x.IdTecAssistance,
                        principalTable: "TechnicalAssistance",
                        principalColumn: "IdTecAssistance",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    IdImage = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Way = table.Column<string>(nullable: true),
                    IdProduct = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.IdImage);
                    table.ForeignKey(
                        name: "FK_Images_Products_IdProduct",
                        column: x => x.IdProduct,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSituation",
                columns: table => new
                {
                    IdOrderSituation = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Situation = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    IdOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSituation", x => x.IdOrderSituation);
                    table.ForeignKey(
                        name: "FK_OrderSituation_Orders_IdOrder",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderAdvertisement",
                columns: table => new
                {
                    IdAdvert = table.Column<int>(nullable: false),
                    IdOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAdvertisement", x => new { x.IdAdvert, x.IdOrder });
                    table.ForeignKey(
                        name: "FK_OrderAdvertisement_Advertisement_IdAdvert",
                        column: x => x.IdAdvert,
                        principalTable: "Advertisement",
                        principalColumn: "IdAdvert",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderAdvertisement_Orders_IdOrder",
                        column: x => x.IdOrder,
                        principalTable: "Orders",
                        principalColumn: "IdOrder",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_IdTecAssistance",
                table: "Address",
                column: "IdTecAssistance");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_IdProduct",
                table: "Advertisement",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_IdTecAssistance",
                table: "Advertisement",
                column: "IdTecAssistance");

            migrationBuilder.CreateIndex(
                name: "IX_Archives_IdTecAssistance",
                table: "Archives",
                column: "IdTecAssistance");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryMasterId",
                table: "Categories",
                column: "CategoryMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_IdTecAssistance",
                table: "Collaborators",
                column: "IdTecAssistance");

            migrationBuilder.CreateIndex(
                name: "IX_Images_IdProduct",
                table: "Images",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAdvertisement_IdOrder",
                table: "OrderAdvertisement",
                column: "IdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdTecAssistance",
                table: "Orders",
                column: "IdTecAssistance");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSituation_IdOrder",
                table: "OrderSituation",
                column: "IdOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IdCategory",
                table: "Products",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IdManufacturer",
                table: "Products",
                column: "IdManufacturer");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalAssistanceManufacturer_IdManufacturer",
                table: "TechnicalAssistanceManufacturer",
                column: "IdManufacturer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Archives");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropTable(
                name: "EventLog");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "OrderAdvertisement");

            migrationBuilder.DropTable(
                name: "OrderSituation");

            migrationBuilder.DropTable(
                name: "TechnicalAssistanceManufacturer");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TechnicalAssistance");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
