using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neptun_backend.Migrations
{
    public partial class uniqueNeptunCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NeptunCode",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NeptunCode",
                table: "Instructors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_NeptunCode",
                table: "Students",
                column: "NeptunCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_NeptunCode",
                table: "Instructors",
                column: "NeptunCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_NeptunCode",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_NeptunCode",
                table: "Instructors");

            migrationBuilder.AlterColumn<string>(
                name: "NeptunCode",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "NeptunCode",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
