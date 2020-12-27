using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreshMart.Data.Migrations
{
    public partial class divisionTableModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Divisions_DivisionId",
                table: "Districts");

            migrationBuilder.AlterColumn<long>(
                name: "DivisionId",
                table: "Districts",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Divisions_DivisionId",
                table: "Districts",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Divisions_DivisionId",
                table: "Districts");

            migrationBuilder.AlterColumn<long>(
                name: "DivisionId",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Divisions_DivisionId",
                table: "Districts",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
