using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace listening.Data.Migrations
{
    public partial class ResultsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 37, nullable: false),
                    TextId = table.Column<string>(maxLength: 25, nullable: false),
                    ResultsEncodedString = table.Column<bool>(maxLength: 8000, nullable: false),
                    Started = table.Column<string>(maxLength: 24, nullable: true),
                    Finished = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
