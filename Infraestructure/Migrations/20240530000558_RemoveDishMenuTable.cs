using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDishMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishMenu");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Dish",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Dish_MenuId",
                table: "Dish",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Menu_MenuId",
                table: "Dish",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Menu_MenuId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_MenuId",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Dish");

            migrationBuilder.CreateTable(
                name: "DishMenu",
                columns: table => new
                {
                    DishesId = table.Column<Guid>(type: "uuid", nullable: false),
                    MenuId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishMenu", x => new { x.DishesId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_DishMenu_Dish_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishMenu_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_MenuId",
                table: "DishMenu",
                column: "MenuId");
        }
    }
}
