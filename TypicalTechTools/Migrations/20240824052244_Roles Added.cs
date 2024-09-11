using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TypicalTechTools.Migrations
{
    public partial class RolesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "AdminId",
                keyValue: 1,
                columns: new[] { "Password", "Role" },
                values: new object[] { "$2a$11$XiHgfI7GW2uR/R9f1nRC5O/KE0c42et3T1LM6Mksocu3zEyzPpvuC", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AdminUsers");

            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "AdminId",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7VYwrcFABlWNA7SMUHkatec5/Mlkch6ww0gCViAc.5bRKecCAK29e");
        }
    }
}
