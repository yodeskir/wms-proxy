using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace wmsDataAccess.Migrations
{
    public partial class mapLogsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "mapslog",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    mapname = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    mapcontent = table.Column<string>(nullable: true),
                    logdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mapslog", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mapslog",
                schema: "public");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:POSTGIS", ",,");
        }
    }
}
