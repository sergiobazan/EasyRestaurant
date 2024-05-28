using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderMenuRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Order_MenuId",
                table: "Order",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Menu_MenuId",
                table: "Order",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Menu_MenuId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_MenuId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Order");
        }
    }
}
