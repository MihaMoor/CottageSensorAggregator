using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgresDb.Migrations
{
    /// <inheritdoc />
    public partial class InitialZontCircuits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zont_circuits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    zont_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    actual_temp = table.Column<double>(type: "double precision", nullable: false),
                    current_temp = table.Column<double>(type: "double precision", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    min = table.Column<int>(type: "integer", nullable: false),
                    max = table.Column<int>(type: "integer", nullable: false),
                    step = table.Column<int>(type: "integer", nullable: false),
                    fetched_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zont_circuits", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_zont_circuits_zont_id",
                table: "zont_circuits",
                column: "zont_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zont_circuits");
        }
    }
}
