using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Output_Caching.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Soda", 1.99m },
                    { 2, "Milk", 3.49m },
                    { 3, "Orange Juice", 4.75m },
                    { 4, "Bread", 2.49m },
                    { 5, "Butter", 3.99m },
                    { 6, "Eggs", 2.99m },
                    { 7, "Cheese", 5.49m },
                    { 8, "Chocolate", 1.99m },
                    { 9, "Coffee", 7.99m },
                    { 10, "Tea", 4.99m },
                    { 11, "Water", 0.99m },
                    { 12, "Fruit Mix Juice", 3.99m },
                    { 13, "Yogurt", 2.99m },
                    { 14, "Cereal", 4.49m },
                    { 15, "Rice", 6.99m },
                    { 16, "Pasta", 3.49m },
                    { 17, "Apple", 0.79m },
                    { 18, "Banana", 0.59m },
                    { 19, "Orange", 0.99m },
                    { 20, "Grapes", 2.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
