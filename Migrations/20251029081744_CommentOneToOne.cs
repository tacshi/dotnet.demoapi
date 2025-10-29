using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CommentOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd7c5818-55ce-4925-a081-cb3a5f90c31f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1917793-f7b3-4914-bbf5-d389e7c7e0fa");

            migrationBuilder.AddColumn<string>(
                name: "AppUserid",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "354879a7-dc26-4a40-9a93-6ca1b0f2232f", null, "User", "USER" },
                    { "448d04b0-dee0-4db3-9e86-963e15ec0ad4", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AppUserid",
                table: "Comments",
                column: "AppUserid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AppUserid",
                table: "Comments",
                column: "AppUserid",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AppUserid",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AppUserid",
                table: "Comments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "354879a7-dc26-4a40-9a93-6ca1b0f2232f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "448d04b0-dee0-4db3-9e86-963e15ec0ad4");

            migrationBuilder.DropColumn(
                name: "AppUserid",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "bd7c5818-55ce-4925-a081-cb3a5f90c31f", null, "User", "USER" },
                    { "f1917793-f7b3-4914-bbf5-d389e7c7e0fa", null, "Admin", "ADMIN" }
                });
        }
    }
}
