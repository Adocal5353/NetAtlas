﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetAtlas.Data;

#nullable disable

namespace NetAtlas.Migrations
{
    [DbContext(typeof(NetAtlasContext))]
    [Migration("20220301175045_AddNullablePP")]
    partial class AddNullablePP
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NetAtlas.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AdminPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("NetAtlas.Models.Membre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("NbrAvertissement")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhotoProfil")
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Membre");
                });

            modelBuilder.Entity("NetAtlas.Models.Moderateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Moderateur");
                });

            modelBuilder.Entity("NetAtlas.Models.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePublication")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("idMemdre")
                        .HasColumnType("int");

                    b.Property<int>("idRessource")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("idMemdre");

                    b.HasIndex("idRessource");

                    b.ToTable("Publication");
                });

            modelBuilder.Entity("NetAtlas.Models.Register", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Register");
                });

            modelBuilder.Entity("NetAtlas.Models.Ressource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("nomRessource")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Ressource");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Ressource");
                });

            modelBuilder.Entity("NetAtlas.Models.Lien", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Lien");
                });

            modelBuilder.Entity("NetAtlas.Models.Message", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<string>("contenu")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Message");
                });

            modelBuilder.Entity("NetAtlas.Models.PhotoVideo", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<float>("TailleEnMo")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("PhotoVideo");
                });

            modelBuilder.Entity("NetAtlas.Models.Publication", b =>
                {
                    b.HasOne("NetAtlas.Models.Membre", "Menber")
                        .WithMany()
                        .HasForeignKey("idMemdre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetAtlas.Models.Ressource", "Res")
                        .WithMany()
                        .HasForeignKey("idRessource")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menber");

                    b.Navigation("Res");
                });
#pragma warning restore 612, 618
        }
    }
}
