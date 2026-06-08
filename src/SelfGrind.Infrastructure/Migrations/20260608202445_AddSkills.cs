using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SelfGrind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Attribute = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    RequiredAttributeLevel = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Attribute", "Description", "Emoji", "Name", "Order", "RequiredAttributeLevel" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0001-000000000001"), "Strength", "Reach Strength level 1", "⚔️", "Warrior I", 1, 1 },
                    { new Guid("22222222-0000-0000-0001-000000000002"), "Strength", "Reach Strength level 5", "🗡️", "Warrior II", 2, 5 },
                    { new Guid("22222222-0000-0000-0001-000000000003"), "Strength", "Reach Strength level 10", "🛡️", "Warrior III", 3, 10 },
                    { new Guid("22222222-0000-0000-0001-000000000004"), "Strength", "Reach Strength level 20", "💥", "Titan", 4, 20 },
                    { new Guid("22222222-0000-0000-0002-000000000001"), "Knowledge", "Reach Knowledge level 1", "📖", "Scholar I", 1, 1 },
                    { new Guid("22222222-0000-0000-0002-000000000002"), "Knowledge", "Reach Knowledge level 5", "📚", "Scholar II", 2, 5 },
                    { new Guid("22222222-0000-0000-0002-000000000003"), "Knowledge", "Reach Knowledge level 10", "🎓", "Scholar III", 3, 10 },
                    { new Guid("22222222-0000-0000-0002-000000000004"), "Knowledge", "Reach Knowledge level 20", "🧙", "Sage", 4, 20 },
                    { new Guid("22222222-0000-0000-0003-000000000001"), "Focus", "Reach Focus level 1", "🧘", "Monk I", 1, 1 },
                    { new Guid("22222222-0000-0000-0003-000000000002"), "Focus", "Reach Focus level 5", "🌿", "Monk II", 2, 5 },
                    { new Guid("22222222-0000-0000-0003-000000000003"), "Focus", "Reach Focus level 10", "☯️", "Monk III", 3, 10 },
                    { new Guid("22222222-0000-0000-0003-000000000004"), "Focus", "Reach Focus level 20", "✨", "Zen Master", 4, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Attribute_Order",
                table: "Skills",
                columns: new[] { "Attribute", "Order" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
