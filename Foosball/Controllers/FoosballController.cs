using Foosball.Logic;
using Foosball.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foosball.Controllers
{
    [Route("[controller]")]
    public class FoosballController : ControllerBase
    {
        private readonly IFoosballService _foosballService;

        public FoosballController(IFoosballService foosballService)
        {
            _foosballService = foosballService;
        }
        
        [HttpGet("games")]
        public FoosballGameDataModel[] GetGames()
        {
            return _foosballService.GetGames();
        }
        
        [HttpGet("games/{id}")]
        public FoosballGameDataModel GetGame(int id)
        {
            return _foosballService.GetGame(id);
        }
        
        [HttpPost("games")]
        public FoosballGameDataModel BookGame([FromBody]BookGameRequestModel model)
        {
            return _foosballService.BookGame(model);
        }
        
        [HttpGet("tables")]
        public TableDataModel[] GetTables()
        {
            return _foosballService.GetTables();
        }

        [HttpPost("games/{gameId}/score")]
        public GameStatus Score(int gameId, [FromBody]ScoreRequestModel model)
        {
            return _foosballService.Score(gameId, model.ScoringTeam);
        }

        [HttpGet("players/{playerId}")]
        public PlayerStatsModel GetPlayerStats(int playerId)
        {
            return _foosballService.GetPlayerStats(playerId);
        }
        
    }
}