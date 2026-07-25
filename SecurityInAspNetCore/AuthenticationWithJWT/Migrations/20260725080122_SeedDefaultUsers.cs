using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationWithJWT.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
