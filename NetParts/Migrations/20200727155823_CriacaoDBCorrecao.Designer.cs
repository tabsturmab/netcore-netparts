﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetParts.Database;

namespace NetParts.Migrations
{
    [DbContext(typeof(NetPartsContext))]
    [Migration("20200727155823_CriacaoDBCorrecao")]
    partial class CriacaoDBCorrecao
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetParts.Models.Address", b =>
                {
                    b.Property<int?>("IdAddress")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Complement")
                        .HasMaxLength(100);

                    b.Property<string>("District")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<int>("IdTecAssistance");

                    b.Property<string>("NumberAta")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("State1")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.Property<string>("ZipCode");

                    b.HasKey("IdAddress");

                    b.HasIndex("IdTecAssistance");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("NetParts.Models.Advertisement", b =>
                {
                    b.Property<int>("IdAdvert")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int>("IdProduct");

                    b.Property<int>("IdTecAssistance");

                    b.Property<double>("Price");

                    b.HasKey("IdAdvert");

                    b.HasIndex("IdProduct");

                    b.HasIndex("IdTecAssistance");

                    b.ToTable("Advertisement");
                });

            modelBuilder.Entity("NetParts.Models.Archive", b =>
                {
                    b.Property<int>("IdArchive")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdTecAssistance");

                    b.Property<string>("Way");

                    b.HasKey("IdArchive");

                    b.HasIndex("IdTecAssistance");

                    b.ToTable("Archives");
                });

            modelBuilder.Entity("NetParts.Models.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryMasterId");

                    b.Property<string>("NameCategory")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.HasKey("IdCategory");

                    b.HasIndex("CategoryMasterId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NetParts.Models.Collaborator", b =>
                {
                    b.Property<int?>("IdCollaborator")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("IdTecAssistance");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("TypeCollaborator")
                        .IsRequired();

                    b.HasKey("IdCollaborator");

                    b.HasIndex("IdTecAssistance");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("NetParts.Models.Image", b =>
                {
                    b.Property<int>("IdImage")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdProduct");

                    b.Property<string>("Way");

                    b.HasKey("IdImage");

                    b.HasIndex("IdProduct");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("NetParts.Models.LogEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<int?>("EventId");

                    b.Property<string>("LogLevel");

                    b.Property<string>("Message");

                    b.HasKey("Id");

                    b.ToTable("EventLog");
                });

            modelBuilder.Entity("NetParts.Models.Manufacturer", b =>
                {
                    b.Property<int>("IdManufacturer")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NameManufacturer")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("IdManufacturer");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("NetParts.Models.Order", b =>
                {
                    b.Property<int>("IdOrder")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DataProducts");

                    b.Property<string>("DataTransaction");

                    b.Property<DateTime>("DateRegisterOrder");

                    b.Property<string>("FormPayment");

                    b.Property<string>("FreightCodTracking");

                    b.Property<string>("FreightCompany");

                    b.Property<int?>("IdTecAssistance");

                    b.Property<string>("NFe");

                    b.Property<string>("Situation");

                    b.Property<string>("TransactionId");

                    b.Property<decimal>("ValueTotal");

                    b.HasKey("IdOrder");

                    b.HasIndex("IdTecAssistance");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("NetParts.Models.OrderAdvertisement", b =>
                {
                    b.Property<int>("IdAdvert");

                    b.Property<int>("IdOrder");

                    b.HasKey("IdAdvert", "IdOrder");

                    b.HasIndex("IdOrder");

                    b.ToTable("OrderAdvertisement");
                });

            modelBuilder.Entity("NetParts.Models.OrderSituation", b =>
                {
                    b.Property<int>("IdOrderSituation")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("IdOrder");

                    b.Property<string>("Situation");

                    b.HasKey("IdOrderSituation");

                    b.HasIndex("IdOrder");

                    b.ToTable("OrderSituation");
                });

            modelBuilder.Entity("NetParts.Models.ProductAggregator.Product", b =>
                {
                    b.Property<int>("IdProduct")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Height");

                    b.Property<int>("IdCategory");

                    b.Property<int>("IdManufacturer");

                    b.Property<int>("Length");

                    b.Property<string>("PartNumber")
                        .IsRequired();

                    b.Property<double>("Weight");

                    b.Property<int>("Width1");

                    b.HasKey("IdProduct");

                    b.HasIndex("IdCategory");

                    b.HasIndex("IdManufacturer");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("NetParts.Models.TechnicalAssistance", b =>
                {
                    b.Property<int?>("IdTecAssistance")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cnpj")
                        .IsRequired();

                    b.Property<DateTime>("DateRegister");

                    b.Property<string>("EmailAta")
                        .IsRequired();

                    b.Property<bool>("EnabledDisabled");

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("SocialReason")
                        .IsRequired();

                    b.Property<string>("StateInscription")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.HasKey("IdTecAssistance");

                    b.ToTable("TechnicalAssistance");
                });

            modelBuilder.Entity("NetParts.Models.TechnicalAssistanceManufacturer", b =>
                {
                    b.Property<int>("IdTecAssistance");

                    b.Property<int>("IdManufacturer");

                    b.HasKey("IdTecAssistance", "IdManufacturer");

                    b.HasIndex("IdManufacturer");

                    b.ToTable("TechnicalAssistanceManufacturer");
                });

            modelBuilder.Entity("NetParts.Models.Address", b =>
                {
                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("Address")
                        .HasForeignKey("IdTecAssistance")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.Advertisement", b =>
                {
                    b.HasOne("NetParts.Models.ProductAggregator.Product", "Product")
                        .WithMany("Advertisement")
                        .HasForeignKey("IdProduct")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("Advertisement")
                        .HasForeignKey("IdTecAssistance")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.Archive", b =>
                {
                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("Archives")
                        .HasForeignKey("IdTecAssistance")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.Category", b =>
                {
                    b.HasOne("NetParts.Models.Category", "CategoryMaster")
                        .WithMany()
                        .HasForeignKey("CategoryMasterId");
                });

            modelBuilder.Entity("NetParts.Models.Collaborator", b =>
                {
                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("Collaborator")
                        .HasForeignKey("IdTecAssistance")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.Image", b =>
                {
                    b.HasOne("NetParts.Models.ProductAggregator.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("IdProduct")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.Order", b =>
                {
                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("Order")
                        .HasForeignKey("IdTecAssistance");
                });

            modelBuilder.Entity("NetParts.Models.OrderAdvertisement", b =>
                {
                    b.HasOne("NetParts.Models.Advertisement", "Advertisement")
                        .WithMany("OrderAdvertisement")
                        .HasForeignKey("IdAdvert")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetParts.Models.Order", "Order")
                        .WithMany("OrderAdvertisement")
                        .HasForeignKey("IdOrder")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.OrderSituation", b =>
                {
                    b.HasOne("NetParts.Models.Order", "Order")
                        .WithMany("OrderSituation")
                        .HasForeignKey("IdOrder");
                });

            modelBuilder.Entity("NetParts.Models.ProductAggregator.Product", b =>
                {
                    b.HasOne("NetParts.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("IdCategory")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetParts.Models.Manufacturer", "Manufacturer")
                        .WithMany("Product")
                        .HasForeignKey("IdManufacturer")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetParts.Models.TechnicalAssistanceManufacturer", b =>
                {
                    b.HasOne("NetParts.Models.Manufacturer", "Manufacturer")
                        .WithMany("TechnicalAssistanceManufacturer")
                        .HasForeignKey("IdManufacturer")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetParts.Models.TechnicalAssistance", "TechnicalAssistance")
                        .WithMany("TechnicalAssistanceManufacturer")
                        .HasForeignKey("IdTecAssistance")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}