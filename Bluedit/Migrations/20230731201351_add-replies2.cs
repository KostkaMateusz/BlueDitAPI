using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bluedit.Migrations
{
    /// <inheritdoc />
    public partial class addreplies2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplyId",
                table: "Replies",
                newName: "ReplayBaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Replies",
                table: "Replies",
                column: "ReplayBaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Replies",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "ReplayBaseId",
                table: "Replies",
                newName: "ReplyId");
        }
    }
}
