using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TokyoGarden.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialPg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    miasto = table.Column<string>(type: "text", nullable: true),
                    nr_domu = table.Column<string>(type: "text", nullable: true),
                    nr_mieszkania = table.Column<string>(type: "text", nullable: true),
                    ulica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Alergeny",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa_alergenu = table.Column<string>(type: "text", nullable: true),
                    opis_alergenu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alergeny", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "KategorieMenu",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa_kategorii = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategorieMenu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nazwa_uzytkownika = table.Column<string>(type: "text", nullable: true),
                    haslo = table.Column<string>(type: "text", nullable: true),
                    telefon = table.Column<string>(type: "text", nullable: true),
                    typ_uzytkownika = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PozycjeMenu",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cena = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    nazwa_pozycji = table.Column<string>(type: "text", nullable: true),
                    opis = table.Column<string>(type: "text", nullable: true),
                    skladniki = table.Column<string>(type: "text", nullable: true),
                    image_data = table.Column<byte[]>(type: "bytea", nullable: true),
                    kategoria_menuid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PozycjeMenu", x => x.id);
                    table.ForeignKey(
                        name: "FK_PozycjeMenu_KategorieMenu_kategoria_menuid",
                        column: x => x.kategoria_menuid,
                        principalTable: "KategorieMenu",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Zamowienia",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data_zamowienia = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    dodatkowe_informacje = table.Column<string>(type: "text", nullable: true),
                    status_zamowienia = table.Column<string>(type: "text", nullable: true),
                    laczna_cena = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    metoda_platnosci = table.Column<string>(type: "text", nullable: true),
                    opcje_zamowienia = table.Column<string>(type: "text", nullable: true),
                    uzytkownikid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienia", x => x.id);
                    table.ForeignKey(
                        name: "FK_Zamowienia_Uzytkownicy_uzytkownikid",
                        column: x => x.uzytkownikid,
                        principalTable: "Uzytkownicy",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AlergenyPozycje_Menu",
                columns: table => new
                {
                    alergenyid = table.Column<int>(type: "integer", nullable: false),
                    pozycje_menuid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlergenyPozycje_Menu", x => new { x.alergenyid, x.pozycje_menuid });
                    table.ForeignKey(
                        name: "FK_AlergenyPozycje_Menu_Alergeny_alergenyid",
                        column: x => x.alergenyid,
                        principalTable: "Alergeny",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlergenyPozycje_Menu_PozycjeMenu_pozycje_menuid",
                        column: x => x.pozycje_menuid,
                        principalTable: "PozycjeMenu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PozycjeZamowienia",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cena = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ilosc = table.Column<int>(type: "integer", nullable: false),
                    zamowienieid = table.Column<int>(type: "integer", nullable: true),
                    pozycja_menuid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PozycjeZamowienia", x => x.id);
                    table.ForeignKey(
                        name: "FK_PozycjeZamowienia_PozycjeMenu_pozycja_menuid",
                        column: x => x.pozycja_menuid,
                        principalTable: "PozycjeMenu",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PozycjeZamowienia_Zamowienia_zamowienieid",
                        column: x => x.zamowienieid,
                        principalTable: "Zamowienia",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlergenyPozycje_Menu_pozycje_menuid",
                table: "AlergenyPozycje_Menu",
                column: "pozycje_menuid");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjeMenu_kategoria_menuid",
                table: "PozycjeMenu",
                column: "kategoria_menuid");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjeZamowienia_pozycja_menuid",
                table: "PozycjeZamowienia",
                column: "pozycja_menuid");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjeZamowienia_zamowienieid",
                table: "PozycjeZamowienia",
                column: "zamowienieid");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienia_uzytkownikid",
                table: "Zamowienia",
                column: "uzytkownikid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adresy");

            migrationBuilder.DropTable(
                name: "AlergenyPozycje_Menu");

            migrationBuilder.DropTable(
                name: "PozycjeZamowienia");

            migrationBuilder.DropTable(
                name: "Alergeny");

            migrationBuilder.DropTable(
                name: "PozycjeMenu");

            migrationBuilder.DropTable(
                name: "Zamowienia");

            migrationBuilder.DropTable(
                name: "KategorieMenu");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");
        }
    }
}
