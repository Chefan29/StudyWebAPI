using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FullTextIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Remarks_Title",
                table: "Remarks",
                column: "Title")
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "russian");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Remarks_Title",
                table: "Remarks");
        }
    }
}
