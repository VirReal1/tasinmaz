using System;
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
                    IlId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IlAdi = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.IlId);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    IlceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IlceAdi = table.Column<string>(type: "text", nullable: true),
                    IlId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.IlceId);
                    table.ForeignKey(
                        name: "FK_Ilceler_Iller_IlId",
                        column: x => x.IlId,
                        principalTable: "Iller",
                        principalColumn: "IlId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogKayitlari",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciIp = table.Column<string>(type: "text", nullable: true),
                    Tarih = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Durum = table.Column<string>(type: "text", nullable: true),
                    IslemTipi = table.Column<string>(type: "text", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    KullaniciId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogKayitlari", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_LogKayitlari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasinmazlar",
                columns: table => new
                {
                    TasinmazId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IlAdi = table.Column<string>(type: "text", nullable: true),
                    IlceAdi = table.Column<string>(type: "text", nullable: true),
                    MahalleAdi = table.Column<string>(type: "text", nullable: true),
                    Ada = table.Column<string>(type: "text", nullable: true),
                    Parsel = table.Column<string>(type: "text", nullable: true),
                    Nitelik = table.Column<string>(type: "text", nullable: true),
                    KoordinatBilgileri = table.Column<string>(type: "text", nullable: true),
                    KullaniciId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasinmazlar", x => x.TasinmazId);
                    table.ForeignKey(
                        name: "FK_Tasinmazlar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mahalleler",
                columns: table => new
                {
                    MahalleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MahalleAdi = table.Column<string>(type: "text", nullable: true),
                    IlceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalleler", x => x.MahalleId);
                    table.ForeignKey(
                        name: "FK_Mahalleler_Ilceler_IlceId",
                        column: x => x.IlceId,
                        principalTable: "Ilceler",
                        principalColumn: "IlceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_IlId",
                table: "Ilceler",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_LogKayitlari_KullaniciId",
                table: "LogKayitlari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahalleler_IlceId",
                table: "Mahalleler",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasinmazlar_KullaniciId",
                table: "Tasinmazlar",
                column: "KullaniciId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogKayitlari");

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
