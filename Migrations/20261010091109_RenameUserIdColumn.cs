using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_tracker.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "users",
                newName: "id");
        }
    }
}
