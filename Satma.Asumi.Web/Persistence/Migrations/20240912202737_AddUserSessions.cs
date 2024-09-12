using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Satma.Asumi.Web.Persistence.Migrations;

public partial class AddUserSessions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "user_sessions",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                refresh_token_id = table.Column<Guid>(type: "uuid", nullable: false),
                expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_sessions", x => x.id);
                table.ForeignKey(
                    name: "fk_user_sessions_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex(
            name: "ix_user_sessions_user_id",
            table: "user_sessions",
            column: "user_id"
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("user_sessions");
    }
}
