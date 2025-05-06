using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoLinkedIn.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "creationDate",
                table: "Posts",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "updateDate",
                table: "Comments",
                newName: "UpdateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Posts",
                newName: "creationDate");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Comments",
                newName: "updateDate");
        }
    }
}
