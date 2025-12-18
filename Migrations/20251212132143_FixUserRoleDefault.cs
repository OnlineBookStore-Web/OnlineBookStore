using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookStore.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
       name: "Role",
       table: "Users",
       type: "nvarchar(50)",
       nullable: false,
       defaultValue: "User",
       oldClrType: typeof(string),
       oldType: "nvarchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
       name: "Role",
       table: "Users",
       type: "nvarchar(50)",
       nullable: false,
       oldClrType: typeof(string),
       oldType: "nvarchar(50)",
       oldDefaultValue: "User");
        }
    }
}
