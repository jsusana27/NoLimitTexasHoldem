using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoLimitTexasHoldemV2.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_CommunityHandDataId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_MachineHandDataId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_PlayerHandDataId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Hands",
                newName: "HandDataId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cards",
                newName: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_CommunityHandDataId",
                table: "Cards",
                column: "CommunityHandDataId",
                principalTable: "Hands",
                principalColumn: "HandDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_MachineHandDataId",
                table: "Cards",
                column: "MachineHandDataId",
                principalTable: "Hands",
                principalColumn: "HandDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_PlayerHandDataId",
                table: "Cards",
                column: "PlayerHandDataId",
                principalTable: "Hands",
                principalColumn: "HandDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_CommunityHandDataId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_MachineHandDataId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Hands_PlayerHandDataId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "HandDataId",
                table: "Hands",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "Cards",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_CommunityHandDataId",
                table: "Cards",
                column: "CommunityHandDataId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_MachineHandDataId",
                table: "Cards",
                column: "MachineHandDataId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Hands_PlayerHandDataId",
                table: "Cards",
                column: "PlayerHandDataId",
                principalTable: "Hands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
