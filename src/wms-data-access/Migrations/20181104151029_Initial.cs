using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace wmsDataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    username = table.Column<string>(nullable: false),
                    useremail = table.Column<string>(nullable: false),
                    userfullname = table.Column<string>(nullable: true),
                    hashedpassword = table.Column<string>(nullable: true),
                    salt = table.Column<string>(nullable: true),
                    creationdate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_DATE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "wmslayers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    username = table.Column<string>(nullable: true),
                    layername = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    extent = table.Column<string>(nullable: true),
                    projection = table.Column<string>(nullable: true),
                    ispublic = table.Column<bool>(nullable: false),
                    layertype = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wmslayers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    mapname = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    center = table.Column<string>(nullable: true),
                    zoom = table.Column<short>(nullable: false),
                    mapprojection = table.Column<string>(nullable: true),
                    mapfile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maps", x => x.id);
                    table.ForeignKey(
                        name: "FK_maps_users_username",
                        column: x => x.username,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_maps_username",
                schema: "public",
                table: "maps",
                column: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maps",
                schema: "public");

            migrationBuilder.DropTable(
                name: "wmslayers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
