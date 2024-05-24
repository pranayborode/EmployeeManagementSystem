using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemProfile_SystemProfile_SystemProfileId",
                table: "SystemProfile");

            migrationBuilder.DropIndex(
                name: "IX_SystemProfile_SystemProfileId",
                table: "SystemProfile");

            migrationBuilder.DropColumn(
                name: "SystemProfileId",
                table: "SystemProfile");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemProfile_ProfileId",
                table: "SystemProfile",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemProfile_SystemProfile_ProfileId",
                table: "SystemProfile",
                column: "ProfileId",
                principalTable: "SystemProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemProfile_SystemProfile_ProfileId",
                table: "SystemProfile");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_SystemProfile_ProfileId",
                table: "SystemProfile");

            migrationBuilder.AddColumn<int>(
                name: "SystemProfileId",
                table: "SystemProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemProfile_SystemProfileId",
                table: "SystemProfile",
                column: "SystemProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemProfile_SystemProfile_SystemProfileId",
                table: "SystemProfile",
                column: "SystemProfileId",
                principalTable: "SystemProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
