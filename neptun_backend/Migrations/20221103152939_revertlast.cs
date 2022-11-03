using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neptun_backend.Migrations
{
    public partial class revertlast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Courses_CourseId",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Instructors_InstructorId",
                table: "CourseInstructor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor");

            migrationBuilder.DropIndex(
                name: "IX_CourseInstructor_CourseId",
                table: "CourseInstructor");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseInstructor");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "CourseInstructor",
                newName: "InstructorsId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseInstructor",
                newName: "CoursesId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseInstructor_InstructorId",
                table: "CourseInstructor",
                newName: "IX_CourseInstructor_InstructorsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor",
                columns: new[] { "CoursesId", "InstructorsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Courses_CoursesId",
                table: "CourseInstructor",
                column: "CoursesId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Instructors_InstructorsId",
                table: "CourseInstructor",
                column: "InstructorsId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Courses_CoursesId",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Instructors_InstructorsId",
                table: "CourseInstructor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor");

            migrationBuilder.RenameColumn(
                name: "InstructorsId",
                table: "CourseInstructor",
                newName: "InstructorId");

            migrationBuilder.RenameColumn(
                name: "CoursesId",
                table: "CourseInstructor",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseInstructor_InstructorsId",
                table: "CourseInstructor",
                newName: "IX_CourseInstructor_InstructorId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseInstructor",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseInstructor",
                table: "CourseInstructor",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructor_CourseId",
                table: "CourseInstructor",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Courses_CourseId",
                table: "CourseInstructor",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Instructors_InstructorId",
                table: "CourseInstructor",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
