using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypicalTechTools.Migrations
{
    public partial class FixedLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "AdminId", "Password", "Username" },
                values: new object[] { 1, "$2a$11$7VYwrcFABlWNA7SMUHkatec5/Mlkch6ww0gCViAc.5bRKecCAK29e", "Admin" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "CommentText", "CreatedDate", "ProductId", "SessionId" },
                values: new object[,]
                {
                    { 1, "This is a great product. Highly Recommended.", null, 12345, null },
                    { 2, "Not worth the excessive price. Stick with a cheaper generic one.", null, 12350, null },
                    { 3, "A great budget buy. As good as some of the expensive alternatives.", null, 12345, null },
                    { 4, "Total garbage. Never buying this brand again. ", null, 12347, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "ProductDescription", "ProductName", "ProductPrice", "UpdatedDate" },
                values: new object[,]
                {
                    { 12345, " bluetooth headphones with fair battery life and a 1 month warranty", " Generic Headphones", 84.99m, null },
                    { 12346, " bluetooth headphones with good battery life and a 6 month warranty", " Expensive Headphones", 149.99m, null },
                    { 12347, " bluetooth headphones with good battery life and a 12 month warranty", " Name Brand Headphones", 199.99m, null },
                    { 12348, " simple bluetooth pointing device", " Generic Wireless Mouse", 39.99m, null },
                    { 12349, " mouse and keyboard wired combination", " Logitach Mouse and Keyboard", 73.99m, null },
                    { 12350, " quality wireless mouse", " Logitach Wireless Mouse", 149.99m, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
