using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bluedit.Migrations
{
    /// <inheritdoc />
    public partial class RenameReplyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplayBaseId",
                table: "Replies",
                newName: "ReplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplyId",
                table: "Replies",
                newName: "ReplayBaseId");
        }
    }
}
