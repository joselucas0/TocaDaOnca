using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TocaDaOnca.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_visitors_Visitors_visitor_id",
                table: "reservations_visitors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visitors",
                table: "Visitors");

            migrationBuilder.RenameTable(
                name: "Visitors",
                newName: "visitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_visitors",
                table: "visitors",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_visitors_visitors_visitor_id",
                table: "reservations_visitors",
                column: "visitor_id",
                principalTable: "visitors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservations_visitors_visitors_visitor_id",
                table: "reservations_visitors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_visitors",
                table: "visitors");

            migrationBuilder.RenameTable(
                name: "visitors",
                newName: "Visitors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visitors",
                table: "Visitors",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_reservations_visitors_Visitors_visitor_id",
                table: "reservations_visitors",
                column: "visitor_id",
                principalTable: "Visitors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
