﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using tasinmaz.API.Data;

namespace tasinmaz.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("tasinmaz.API.Models.Il", b =>
                {
                    b.Property<int>("IlId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("IlAdi")
                        .HasColumnType("text");

                    b.HasKey("IlId");

                    b.ToTable("Iller");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Ilce", b =>
                {
                    b.Property<int>("IlceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("IlId")
                        .HasColumnType("integer");

                    b.Property<string>("IlceAdi")
                        .HasColumnType("text");

                    b.HasKey("IlceId");

                    b.HasIndex("IlId");

                    b.ToTable("Ilceler");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Kullanici", b =>
                {
                    b.Property<int>("KullaniciId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Ad")
                        .HasColumnType("text");

                    b.Property<bool>("AdminMi")
                        .HasColumnType("boolean");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("Soyad")
                        .HasColumnType("text");

                    b.HasKey("KullaniciId");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("tasinmaz.API.Models.LogKaydi", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Aciklama")
                        .HasColumnType("text");

                    b.Property<string>("Durum")
                        .HasColumnType("text");

                    b.Property<string>("IslemTipi")
                        .HasColumnType("text");

                    b.Property<int?>("KullaniciId")
                        .HasColumnType("integer");

                    b.Property<string>("KullaniciIp")
                        .HasColumnType("text");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("LogId");

                    b.HasIndex("KullaniciId");

                    b.ToTable("LogKayitlari");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Mahalle", b =>
                {
                    b.Property<int>("MahalleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("IlceId")
                        .HasColumnType("integer");

                    b.Property<string>("MahalleAdi")
                        .HasColumnType("text");

                    b.HasKey("MahalleId");

                    b.HasIndex("IlceId");

                    b.ToTable("Mahalleler");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Tasinmaz", b =>
                {
                    b.Property<int>("TasinmazId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Ada")
                        .HasColumnType("text");

                    b.Property<string>("IlAdi")
                        .HasColumnType("text");

                    b.Property<string>("IlceAdi")
                        .HasColumnType("text");

                    b.Property<string>("KoordinatBilgileri")
                        .HasColumnType("text");

                    b.Property<int?>("KullaniciId")
                        .HasColumnType("integer");

                    b.Property<string>("MahalleAdi")
                        .HasColumnType("text");

                    b.Property<string>("Nitelik")
                        .HasColumnType("text");

                    b.Property<string>("Parsel")
                        .HasColumnType("text");

                    b.HasKey("TasinmazId");

                    b.HasIndex("KullaniciId");

                    b.ToTable("Tasinmazlar");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Ilce", b =>
                {
                    b.HasOne("tasinmaz.API.Models.Il", "Il")
                        .WithMany()
                        .HasForeignKey("IlId");

                    b.Navigation("Il");
                });

            modelBuilder.Entity("tasinmaz.API.Models.LogKaydi", b =>
                {
                    b.HasOne("tasinmaz.API.Models.Kullanici", "Kullanici")
                        .WithMany()
                        .HasForeignKey("KullaniciId");

                    b.Navigation("Kullanici");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Mahalle", b =>
                {
                    b.HasOne("tasinmaz.API.Models.Ilce", "Ilce")
                        .WithMany()
                        .HasForeignKey("IlceId");

                    b.Navigation("Ilce");
                });

            modelBuilder.Entity("tasinmaz.API.Models.Tasinmaz", b =>
                {
                    b.HasOne("tasinmaz.API.Models.Kullanici", "Kullanici")
                        .WithMany()
                        .HasForeignKey("KullaniciId");

                    b.Navigation("Kullanici");
                });
#pragma warning restore 612, 618
        }
    }
}
