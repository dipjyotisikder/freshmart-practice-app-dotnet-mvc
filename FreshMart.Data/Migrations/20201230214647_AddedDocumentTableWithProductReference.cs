using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreshMart.Data.Migrations
{
    public partial class AddedDocumentTableWithProductReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PhotoId",
                table: "Products",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PhotoId",
                table: "Products",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Documents_PhotoId",
                table: "Products",
                column: "PhotoId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Documents_PhotoId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_PhotoId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Products");
        }
    }
}
