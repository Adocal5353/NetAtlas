﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetAtlas.Data;

#nullable disable

namespace NetAtlas.Migrations
{
    [DbContext(typeof(NetAtlasContext))]
    partial class NetAtlasContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nom")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("isLogged")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("NetAtlas.Models.Amitie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("IdReceiver")
                        .HasColumnType("int");

                    b.Property<int>("IdSender")
                        .HasColumnType("int");

                    b.Property<int>("Statut")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdReceiver");

                    b.HasIndex("IdSender");

                    b.ToTable("Amitie");
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
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhotoProfil")
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("isLogged")
                        .HasColumnType("int");

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
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .HasColumnType("longtext");

                    b.Property<int>("isLogged")
                        .HasColumnType("int");

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

                    b.Property<int>("IdMemdre")
                        .HasColumnType("int");

                    b.Property<bool>("etat")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("IdMemdre");

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
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Prenom")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

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

                    b.Property<int>("IdPublication")
                        .HasColumnType("int");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<string>("nomRessource")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IdPublication");

                    b.ToTable("Ressource");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Ressource");
                });

            modelBuilder.Entity("NetAtlas.Models.Lien", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Lien");
                });

            modelBuilder.Entity("NetAtlas.Models.Message", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<string>("contenu")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Message");
                });

            modelBuilder.Entity("NetAtlas.Models.PhotoVideo", b =>
                {
                    b.HasBaseType("NetAtlas.Models.Ressource");

                    b.Property<string>("Chemin")
                        .HasColumnType("longtext");

                    b.Property<float>("TailleEnMo")
                        .HasColumnType("float");

                    b.Property<int>("TypeMedia")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("PhotoVideo");
                });

            modelBuilder.Entity("NetAtlas.Models.Amitie", b =>
                {
                    b.HasOne("NetAtlas.Models.Membre", "Receiver")
                        .WithMany()
                        .HasForeignKey("IdReceiver")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetAtlas.Models.Membre", "Sender")
                        .WithMany()
                        .HasForeignKey("IdSender")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("NetAtlas.Models.Publication", b =>
                {
                    b.HasOne("NetAtlas.Models.Membre", "Menber")
                        .WithMany()
                        .HasForeignKey("IdMemdre")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menber");
                });

            modelBuilder.Entity("NetAtlas.Models.Ressource", b =>
                {
                    b.HasOne("NetAtlas.Models.Publication", "Publication")
                        .WithMany()
                        .HasForeignKey("IdPublication")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publication");
                });
#pragma warning restore 612, 618
        }
    }
}
