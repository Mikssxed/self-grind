using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SelfGrind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEvolutionTiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvolutionTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    MinLevel = table.Column<int>(type: "int", nullable: false),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    NextEvolutionLabel = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionTiers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EvolutionTiers",
                columns: new[] { "Id", "Emoji", "MaxLevel", "MinLevel", "Name", "NextEvolutionLabel", "Order", "StageName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-000000000001"), "🌱", 10, 1, "Novice", "Level 11: Apprentice", 1, "Novice Adventurer" },
                    { new Guid("11111111-1111-1111-1111-000000000002"), "📖", 20, 11, "Apprentice", "Level 21: Adept", 2, "Apprentice" },
                    { new Guid("11111111-1111-1111-1111-000000000003"), "⚡", 30, 21, "Adept", "Level 31: Expert", 3, "Adept" },
                    { new Guid("11111111-1111-1111-1111-000000000004"), "🔥", 40, 31, "Expert", "Level 41: Master", 4, "Elite Warrior" },
                    { new Guid("11111111-1111-1111-1111-000000000005"), "👑", 50, 41, "Master", null, 5, "Legendary Champion" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionTiers_Order",
                table: "EvolutionTiers",
                column: "Order",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvolutionTiers");
        }
    }
}
