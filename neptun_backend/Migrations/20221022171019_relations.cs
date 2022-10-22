using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neptun_backend.Migrations
{
    public partial class relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Semesters",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Instructors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Courses",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "Courseid",
                table: "Instructors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_Courseid",
                table: "Instructors",
                column: "Courseid");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Courses_Courseid",
                table: "Instructors",
                column: "Courseid",
                principalTable: "Courses",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Courses_Courseid",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_Courseid",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Courseid",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Semesters",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Instructors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Courses",
                newName: "Id");
        }
    }
}
