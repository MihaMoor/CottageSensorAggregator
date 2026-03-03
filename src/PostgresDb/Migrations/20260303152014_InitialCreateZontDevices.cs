using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PostgresDb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateZontDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zont_devices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    zont_id = table.Column<int>(type: "integer", nullable: false),
                    device_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_online = table.Column<bool>(type: "boolean", nullable: false),
                    device_model = table.Column<string>(type: "text", nullable: false),
                    software_version = table.Column<string>(type: "text", nullable: false),
                    hardware_version = table.Column<string>(type: "text", nullable: false),
                    fetched_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zont_devices", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_zont_devices_zont_id",
                table: "zont_devices",
                column: "zont_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zont_devices");
        }
    }
}
