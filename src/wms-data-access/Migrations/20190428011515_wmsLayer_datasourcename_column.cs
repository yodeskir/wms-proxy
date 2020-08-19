using Microsoft.EntityFrameworkCore.Migrations;

namespace wmsDataAccess.Migrations
{
    public partial class wmsLayer_datasourcename_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "datasourcename",
                schema: "public",
                table: "wmslayers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "datasourcename",
                schema: "public",
                table: "wmslayers");
        }
    }
}
