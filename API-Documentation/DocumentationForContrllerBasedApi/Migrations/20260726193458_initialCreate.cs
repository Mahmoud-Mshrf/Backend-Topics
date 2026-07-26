using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DocumentationForContrllerBasedApi.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.AppUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "LastName", "PasswordHash", "Permissions", "Roles" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateOnly(1995, 1, 1), "supermanager@test.com", "Super", "Manager", "AQAAAAIAAYagAAAAEMgowEOmfyX1Fg2pVWG9Y3g9EmziM9VtE4sFqvfIMrhOiy/RLoXtfnaNNHd1FSFSTg==", "[\"project:create\",\"project:read\",\"project:update\",\"project:delete\",\"project:assign_member\",\"project:manage_budget\",\"task:create\",\"task:read\",\"task:update\",\"task:delete\",\"task:assign_user\",\"task:update_status\",\"task:comment\"]", "[\"supermanager\"]" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateOnly(1997, 1, 1), "manager@test.com", "Project", "Manager", "AQAAAAIAAYagAAAAENKcW6STYSBPHp/ZOs3E/hxeH1r06H3kZ0Eltq1NnY9oZ96w5UGAT0z6Fy8QNFqHpA==", "[\"project:create\",\"project:read\",\"project:update\",\"project:delete\",\"project:assign_member\",\"project:manage_budget\"]", "[\"manager\"]" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateOnly(2000, 1, 1), "employee@test.com", "Employee", "User", "AQAAAAIAAYagAAAAEEN611uY7ftj3llpfZo2d8zCn9kyo0Zxqxb/AswL3uWQYFNjFegvCupnkaGUjU4NJw==", "[\"task:create\",\"task:read\",\"task:update\",\"task:delete\",\"task:assign_user\",\"task:update_status\",\"task:comment\"]", "[\"employee\"]" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
