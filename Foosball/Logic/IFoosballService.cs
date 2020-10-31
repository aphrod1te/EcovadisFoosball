using Foosball.Models;

namespace Foosball.Logic
{
    public interface IFoosballService
    {
        FoosballGameDataModel GetGame(int gameId);
        FoosballGameDataModel[] GetGames();
        FoosballGameDataModel BookGame(BookGameRequestModel model);
        TableDataModel[] GetTables();
        GameStatus Score(int gameId, ScoringTeam scoringTeam);
        PlayerStatsModel GetPlayerStats(int playerId);
    }
}