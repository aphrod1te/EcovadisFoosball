using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foosball.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoosballGameScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballGameScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoosballTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoosballTeamPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballTeamPlayers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoosballTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballTeams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoosballGameSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamAScore = table.Column<int>(nullable: false),
                    TeamBScore = table.Column<int>(nullable: false),
                    FoosballGameScoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballGameSets", x => x.Id);
                    table.CheckConstraint("TeamAScore_Positive", "[TeamAScore] >= 0");
                    table.CheckConstraint("TeamBScore_Positive", "[TeamBScore] >= 0");
                    table.ForeignKey(
                        name: "FK_FoosballGameSets_FoosballGameScores_FoosballGameScoreId",
                        column: x => x.FoosballGameScoreId,
                        principalTable: "FoosballGameScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoosballGames",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TableId = table.Column<int>(nullable: false),
                    StartDate = table.Column<long>(nullable: false),
                    EndDate = table.Column<long>(nullable: true),
                    FoosballGameScoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballGames", x => x.Id);
                    table.CheckConstraint("CK_EndDate_GreaterThan_StartDate", "[EndDate] > [StartDate]");
                    table.CheckConstraint("CK_StartDate_In_Future", "[StartDate] > CAST((((JulianDay('now', 'localtime') - 2440587.5)*86400.0) + 62135596800) * 10000000 AS BIGINT)");
                    table.ForeignKey(
                        name: "FK_FoosballGames_FoosballGameScores_FoosballGameScoreId",
                        column: x => x.FoosballGameScoreId,
                        principalTable: "FoosballGameScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoosballGames_FoosballTables_TableId",
                        column: x => x.TableId,
                        principalTable: "FoosballTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoosballTeamPlayer",
                columns: table => new
                {
                    FoosballTeamId = table.Column<int>(nullable: false),
                    FoosballPlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballTeamPlayer", x => new { x.FoosballPlayerId, x.FoosballTeamId });
                    table.ForeignKey(
                        name: "FK_FoosballTeamPlayer_FoosballTeamPlayers_FoosballPlayerId",
                        column: x => x.FoosballPlayerId,
                        principalTable: "FoosballTeamPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoosballTeamPlayer_FoosballTeams_FoosballTeamId",
                        column: x => x.FoosballTeamId,
                        principalTable: "FoosballTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoosballGameFinishedSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FoosballGameSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballGameFinishedSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoosballGameFinishedSets_FoosballGameSets_FoosballGameSetId",
                        column: x => x.FoosballGameSetId,
                        principalTable: "FoosballGameSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoosballPlayerSetups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FoosballGameId = table.Column<int>(nullable: false),
                    TeamAId = table.Column<int>(nullable: true),
                    TeamBId = table.Column<int>(nullable: true),
                    FoosballGameId1 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoosballPlayerSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoosballPlayerSetups_FoosballGames_FoosballGameId",
                        column: x => x.FoosballGameId,
                        principalTable: "FoosballGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoosballPlayerSetups_FoosballTeams_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "FoosballTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FoosballPlayerSetups_FoosballTeams_TeamBId",
                        column: x => x.TeamBId,
                        principalTable: "FoosballTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FoosballTables",
                column: "Id",
                value: 1);

            migrationBuilder.InsertData(
                table: "FoosballTables",
                column: "Id",
                value: 2);

            migrationBuilder.InsertData(
                table: "FoosballTeamPlayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "First" });

            migrationBuilder.InsertData(
                table: "FoosballTeamPlayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Second" });

            migrationBuilder.InsertData(
                table: "FoosballTeamPlayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Third" });

            migrationBuilder.InsertData(
                table: "FoosballTeamPlayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "Fourth" });

            migrationBuilder.CreateIndex(
                name: "IX_FoosballGameFinishedSets_FoosballGameSetId",
                table: "FoosballGameFinishedSets",
                column: "FoosballGameSetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoosballGames_FoosballGameScoreId",
                table: "FoosballGames",
                column: "FoosballGameScoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Available_Table",
                table: "FoosballGames",
                column: "TableId",
                unique: true,
                filter: "[EndDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FoosballGames_TableId_StartDate",
                table: "FoosballGames",
                columns: new[] { "TableId", "StartDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoosballGameSets_FoosballGameScoreId",
                table: "FoosballGameSets",
                column: "FoosballGameScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_FoosballPlayerSetups_FoosballGameId",
                table: "FoosballPlayerSetups",
                column: "FoosballGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoosballPlayerSetups_TeamAId",
                table: "FoosballPlayerSetups",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_FoosballPlayerSetups_TeamBId",
                table: "FoosballPlayerSetups",
                column: "TeamBId");

            migrationBuilder.CreateIndex(
                name: "IX_FoosballTeamPlayer_FoosballTeamId",
                table: "FoosballTeamPlayer",
                column: "FoosballTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoosballGameFinishedSets");

            migrationBuilder.DropTable(
                name: "FoosballPlayerSetups");

            migrationBuilder.DropTable(
                name: "FoosballTeamPlayer");

            migrationBuilder.DropTable(
                name: "FoosballGameSets");

            migrationBuilder.DropTable(
                name: "FoosballGames");

            migrationBuilder.DropTable(
                name: "FoosballTeamPlayers");

            migrationBuilder.DropTable(
                name: "FoosballTeams");

            migrationBuilder.DropTable(
                name: "FoosballGameScores");

            migrationBuilder.DropTable(
                name: "FoosballTables");
        }
    }
}
