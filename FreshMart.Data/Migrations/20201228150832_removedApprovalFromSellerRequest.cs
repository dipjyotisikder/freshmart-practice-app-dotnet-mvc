using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FreshMart.Data.Migrations
{
    public partial class removedApprovalFromSellerRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval",
                table: "Sellers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approval",
                table: "Sellers",
                nullable: true);
        }
    }
}
