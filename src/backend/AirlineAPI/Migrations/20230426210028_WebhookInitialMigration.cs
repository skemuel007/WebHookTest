using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirlineAPI.Migrations
{
    public partial class WebhookInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebHookSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebHookUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebHookType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebHookPublisher = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookSubscription", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebHookSubscription");
        }
    }
}
