using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoLimitTexasHoldemV2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HandDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerInitialStack = table.Column<int>(type: "int", nullable: false),
                    MachineInitialStack = table.Column<int>(type: "int", nullable: false),
                    Bet = table.Column<int>(type: "int", nullable: false),
                    PlayerHandRank = table.Column<int>(type: "int", nullable: false),
                    MachineHandRank = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerHandDataId = table.Column<int>(type: "int", nullable: true),
                    MachineHandDataId = table.Column<int>(type: "int", nullable: true),
                    CommunityHandDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Hands_CommunityHandDataId",
                        column: x => x.CommunityHandDataId,
                        principalTable: "Hands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Hands_MachineHandDataId",
                        column: x => x.MachineHandDataId,
                        principalTable: "Hands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Hands_PlayerHandDataId",
                        column: x => x.PlayerHandDataId,
                        principalTable: "Hands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CommunityHandDataId",
                table: "Cards",
                column: "CommunityHandDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_MachineHandDataId",
                table: "Cards",
                column: "MachineHandDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PlayerHandDataId",
                table: "Cards",
                column: "PlayerHandDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Hands");
        }
    }
}
