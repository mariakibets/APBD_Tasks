﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tutorial_11.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenExpiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshToken");
        }
    }
}
