using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bluedit.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationDateToReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Replies",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Replies");
        }
    }
}
