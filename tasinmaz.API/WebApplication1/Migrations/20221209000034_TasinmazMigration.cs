using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace tasinmaz.API.Migrations
{
    public partial class TasinmazMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Iller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Adi = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: true),
                    Soyad = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    AdminMi = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Adi = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ilceler_Iller_Id",
                        column: x => x.Id,
                        principalTable: "Iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasinmazlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Adi = table.Column<string>(type: "text", nullable: false),
                    IlAdi = table.Column<string>(type: "text", nullable: true),
                    IlceAdi = table.Column<string>(type: "text", nullable: true),
                    MahalleAdi = table.Column<string>(type: "text", nullable: true),
                    Ada = table.Column<string>(type: "text", nullable: true),
                    Parsel = table.Column<string>(type: "text", nullable: true),
                    Nitelik = table.Column<string>(type: "text", nullable: true),
                    KoordinatBilgileri = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasinmazlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasinmazlar_Kullanicilar_Id",
                        column: x => x.Id,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mahalleler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Adi = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalleler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mahalleler_Ilceler_Id",
                        column: x => x.Id,
                        principalTable: "Ilceler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loglar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciIp = table.Column<string>(type: "text", nullable: true),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Durum = table.Column<string>(type: "text", nullable: true),
                    Islem = table.Column<string>(type: "text", nullable: true),
                    Mesaj = table.Column<string>(type: "text", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    VeriId = table.Column<int>(type: "integer", nullable: true),
                    HataMesajlari = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loglar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loglar_Tasinmazlar_VeriId",
                        column: x => x.VeriId,
                        principalTable: "Tasinmazlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loglar_VeriId",
                table: "Loglar",
                column: "VeriId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loglar");

            migrationBuilder.DropTable(
                name: "Mahalleler");

            migrationBuilder.DropTable(
                name: "Tasinmazlar");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Iller");
        }
    }
}
