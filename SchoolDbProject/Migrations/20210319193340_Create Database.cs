using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolDbProject.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cabinet",
                columns: table => new
                {
                    CabinetId = table.Column<int>(type: "int", nullable: false),
                    NumberOfSeats = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabinet", x => x.CabinetId);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ClassName = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true),
                    NumberOfStudents = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SubjectName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    LastName = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.TeacherId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    LastName = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK__Student__ClassId__2A4B4B5E",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentSchedule",
                columns: table => new
                {
                    LessonNumber = table.Column<byte>(type: "tinyint", nullable: true),
                    DayOfWeek = table.Column<byte>(type: "tinyint", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    CabinetId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__StudentSc__Cabin__30F848ED",
                        column: x => x.CabinetId,
                        principalTable: "Cabinet",
                        principalColumn: "CabinetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__StudentSc__Subje__31EC6D26",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__StudentSc__Teach__300424B4",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__StudentSc__Teach__32E0915F",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mark",
                columns: table => new
                {
                    Mark = table.Column<byte>(type: "tinyint", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__Mark__StudentId__2C3393D0",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mark__SubjectId__3E52440B",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mark_StudentId",
                table: "Mark",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Mark_SubjectId",
                table: "Mark",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ClassId",
                table: "Student",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedule_CabinetId",
                table: "StudentSchedule",
                column: "CabinetId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedule_ClassId",
                table: "StudentSchedule",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedule_SubjectId",
                table: "StudentSchedule",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSchedule_TeacherId",
                table: "StudentSchedule",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mark");

            migrationBuilder.DropTable(
                name: "StudentSchedule");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Cabinet");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Teacher");

            migrationBuilder.DropTable(
                name: "Class");
        }
    }
}
