using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MangoFusion_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Appetizer", "Crispy and savory Indian pastry filled with spiced potatoes and peas.", "https://mangofusionapi.blob.core.windows.net/mango/1.jpg", "Samosa", 15.0, "Chef's Special" },
                    { 2, "Appetizer", "Marinated cubes of paneer cheese grilled to perfection, served with mint chutney.", "https://mangofusionapi.blob.core.windows.net/mango/2.jpg", "Paneer Tikka", 13.99, "" },
                    { 3, "Main Course", "Tender chicken pieces cooked in a creamy tomato-based sauce, served with basmati rice.", "https://mangofusionapi.blob.core.windows.net/mango/3.jpg", "Chicken Tikka Masala", 20.989999999999998, "Chef's Special" },
                    { 4, "Bread", "Soft and fluffy Indian bread topped with garlic and butter, perfect for scooping up curries.", "https://mangofusionapi.blob.core.windows.net/mango/4.jpg", "Garlic Naan", 5.9900000000000002, "" },
                    { 5, "Beverage", "A refreshing yogurt-based drink blended with ripe mangoes and a hint of cardamom.", "https://mangofusionapi.blob.core.windows.net/mango/5.jpg", "Mango Lassi", 7.9900000000000002, "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
