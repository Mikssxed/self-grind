using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SelfGrind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Bonus = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Variant = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    UnlockLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false),
                    AcquiredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Bonus", "Emoji", "Name", "Rarity", "Type", "UnlockLevel", "Variant" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), "+10% Energy", "🧪", "Health Potion", "Common", "Consumable", 1, "Success" },
                    { new Guid("33333333-0000-0000-0000-000000000002"), "+15 Knowledge XP", "📜", "Wisdom Scroll", "Uncommon", "Artifact", 1, "Info" },
                    { new Guid("33333333-0000-0000-0000-000000000003"), "+25 Focus XP", "📿", "Focus Amulet", "Rare", "Artifact", 3, "Violet" },
                    { new Guid("33333333-0000-0000-0000-000000000004"), "+20% Strength", "💎", "Power Band", "Epic", "Accessory", 5, "Error" },
                    { new Guid("33333333-0000-0000-0000-000000000005"), "+20 Strength", "🧤", "Power Gauntlets", "Epic", "Gloves", 5, "Error" },
                    { new Guid("33333333-0000-0000-0000-000000000006"), "+30 Vitality", "💚", "Life Amulet", "Rare", "Artifact", 4, "Success" },
                    { new Guid("33333333-0000-0000-0000-000000000007"), "+50% XP Gain", "⭐", "XP Booster", "Legendary", "Boost", 10, "Warning" },
                    { new Guid("33333333-0000-0000-0000-000000000008"), "+22 Energy", "⚡", "Energy Crystal", "Uncommon", "Consumable", 2, "Warning" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemId",
                table: "UserItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_UserId_ItemId",
                table: "UserItems",
                columns: new[] { "UserId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
