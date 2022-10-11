using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrcture.Migrations
{
    public partial class waletnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "walets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    senderphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sendername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resiverphone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resivername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variz = table.Column<int>(type: "int", nullable: false),
                    bardasht = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_walets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "walets");
        }
    }
}
