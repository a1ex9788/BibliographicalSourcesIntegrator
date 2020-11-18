﻿// <auto-generated />
using System;
using BibliographicalSourcesIntegratorWarehouse.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BibliographicalSourcesIntegratorWarehouse.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Exemplar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("JournalId")
                        .HasColumnType("int");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Volume")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("JournalId");

                    b.ToTable("Exemplars");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Journal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Journals");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surnames")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Person_Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("PublicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("PublicationId");

                    b.ToTable("Person_Publication");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Publications");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Publication");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Article", b =>
                {
                    b.HasBaseType("BibliographicalSourcesIntegratorWarehouse.Entities.Publication");

                    b.Property<int?>("ExemplarId")
                        .HasColumnType("int");

                    b.Property<string>("FinalPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitialPage")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("ExemplarId");

                    b.HasDiscriminator().HasValue("Article");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Book", b =>
                {
                    b.HasBaseType("BibliographicalSourcesIntegratorWarehouse.Entities.Publication");

                    b.Property<string>("Editorial")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.CongressComunication", b =>
                {
                    b.HasBaseType("BibliographicalSourcesIntegratorWarehouse.Entities.Publication");

                    b.Property<string>("Congress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Edition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FinalPage")
                        .HasColumnName("CongressComunication_FinalPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InitialPage")
                        .HasColumnName("CongressComunication_InitialPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Place")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("CongressComunication");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Exemplar", b =>
                {
                    b.HasOne("BibliographicalSourcesIntegratorWarehouse.Entities.Journal", "Journal")
                        .WithMany("Exemplars")
                        .HasForeignKey("JournalId");
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Person_Publication", b =>
                {
                    b.HasOne("BibliographicalSourcesIntegratorWarehouse.Entities.Person", "Person")
                        .WithMany("Publications")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BibliographicalSourcesIntegratorWarehouse.Entities.Publication", "Publication")
                        .WithMany("People")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BibliographicalSourcesIntegratorWarehouse.Entities.Article", b =>
                {
                    b.HasOne("BibliographicalSourcesIntegratorWarehouse.Entities.Exemplar", "Exemplar")
                        .WithMany("Articles")
                        .HasForeignKey("ExemplarId");
                });
#pragma warning restore 612, 618
        }
    }
}
