using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations.EfDb
{
    /// <inheritdoc />
    public partial class CreateEfDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizeRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "ID", "AuthorizeRole", "Content", "MenuName", "Title" },
                values: new object[,]
                {
                    { "008a6b4a-7c9e-4f19-b1ea-30c2f39ef5d4", "software", "The Software Department creates and maintains software for innovation and efficiency. They use latest technology and deliver high-quality products. The team is essential for the success and growth of the company.", "software Department", "Software Department: Driving Innovation and Efficiency through Technology" },
                    { "01042040-543f-4520-b07c-fb66da39db59", "accounting", "The Accounting Department handles financial transactions and record-keeping. They provide financial analysis and ensure compliance with laws and ethics. The team is crucial to the financial stability of the organization.", "accounting Department", "Accounting Department: Ensuring Safe and Accurate Management of Financial Transactions" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
