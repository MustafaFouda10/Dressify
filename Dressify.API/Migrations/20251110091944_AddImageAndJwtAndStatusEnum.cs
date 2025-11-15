using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dressify.API.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndJwtAndStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Dresses");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Dresses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Dresses");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Dresses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
