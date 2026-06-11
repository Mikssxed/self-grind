using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfGrind.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_HabitEntries_HabitId",
                table: "HabitEntries");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId_IsArchived",
                table: "Tasks",
                columns: new[] { "UserId", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskOccurrences_Status_CompletedDate",
                table: "TaskOccurrences",
                columns: new[] { "Status", "CompletedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskOccurrences_Status_ScheduledDate",
                table: "TaskOccurrences",
                columns: new[] { "Status", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_HabitEntries_HabitId_EntryDate",
                table: "HabitEntries",
                columns: new[] { "HabitId", "EntryDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId_IsArchived",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskOccurrences_Status_CompletedDate",
                table: "TaskOccurrences");

            migrationBuilder.DropIndex(
                name: "IX_TaskOccurrences_Status_ScheduledDate",
                table: "TaskOccurrences");

            migrationBuilder.DropIndex(
                name: "IX_HabitEntries_HabitId_EntryDate",
                table: "HabitEntries");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitEntries_HabitId",
                table: "HabitEntries",
                column: "HabitId");
        }
    }
}
