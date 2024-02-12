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
                keyValue: "352d92cf-762b-47bd-92a0-5c9332c0d8b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44d56e20-30a0-4647-be1c-ce60f398cf1a");

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
                    { "1b3959b4-dc1a-4017-82d2-4e9f04d8c9f6", null, "Admin", "ADMIN" },
                    { "49673f1a-1175-46c8-a243-4836f7445192", null, "USER", "USER" }
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
                keyValue: "1b3959b4-dc1a-4017-82d2-4e9f04d8c9f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49673f1a-1175-46c8-a243-4836f7445192");

            migrationBuilder.DropColumn(
                name: "AppUserid",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "352d92cf-762b-47bd-92a0-5c9332c0d8b0", null, "Admin", "ADMIN" },
                    { "44d56e20-30a0-4647-be1c-ce60f398cf1a", null, "USER", "USER" }
                });
        }
    }
}
