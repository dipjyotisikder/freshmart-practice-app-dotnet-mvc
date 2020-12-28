using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreshMart.Data.Migrations
{
    public partial class removedDistrictFromSellerRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerRequests_Districts_DistrictId",
                table: "SellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_SellerRequests_DistrictId",
                table: "SellerRequests");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "SellerRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DistrictId",
                table: "SellerRequests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SellerRequests_DistrictId",
                table: "SellerRequests",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerRequests_Districts_DistrictId",
                table: "SellerRequests",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
