using System;
using System.Linq;
using Foosball.Logic.Persistence;
using Foosball.Logic.Persistence.Entities;
using Foosball.Models;
using Foosball.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Foosball.Logic
{
    public class FoosballService : IFoosballService
    {
        private const int maxScore = 10;
        private const int maxSetCount = 2;
        
        private readonly FoosballContext _context;

        public FoosballService(FoosballContext context)
        {
            _context = context;
        }

        public FoosballGameDataModel BookGame(BookGameRequestModel model)
        {
            var tableId =
                model.TableId ?? GetAvailableTableId();
            var builder
                = new FoosballGameBuilder(_context, tableId)
                    .SetStartDate(DateTimeOffset.UtcNow);
            if (model.TeamSetup is TeamSetup setup && setup.TeamAPlayers.Length > 0 &&
                setup.TeamBPlayers.Length > 0)
                builder.SetTeams(setup.TeamAPlayers, setup.TeamBPlayers);
            var game = builder.Build();
            _context.FoosballGames.Add(game);
            _context.SaveChanges();
            return new FoosballGameDataModel
            {
                Id = game.Id,
                TableId = tableId,
                StartDate = game.StartDate,
                TeamAPlayerIds = game.FoosballPlayerSetup?.TeamA?.FoosballTeamPlayers?.Select(x => x.FoosballPlayerId)
                    .ToList(),
                TeamBPlayerIds = game.FoosballPlayerSetup?.TeamB?.FoosballTeamPlayers?.Select(x => x.FoosballPlayerId)
                    .ToList()
            };
        }

        private int GetAvailableTableId()
        {
            var now = DateTimeOffset.UtcNow;
            var availableTable
                = _context
                    .FoosballTables
                    .AsNoTracking()
                    .FirstOrDefault(x => !x.FoosballGames.Any(x => x.EndDate == null || x.EndDate > now));

            return availableTable.Id;
        }

        public TableDataModel[] GetTables()
        {
            var now = DateTimeOffset.UtcNow;
            var tables =
                _context
                    .FoosballTables
                    .AsNoTracking()
                    .Select(x => new TableDataModel
                    {
                        Id = x.Id,
                        IsAvailable = x.FoosballGames.All(x => x.EndDate != null && x.EndDate < now)
                    })
                    .ToArray();
            return tables;
        }

        public GameStatus Score(int gameId, ScoringTeam scoringTeam)
        {
            var game = _context.FoosballGames.Include(x => x.Score.Sets).ThenInclude(x => x.FinishedSet).FirstOrDefault(x => x.Id == gameId);
            if (game.EndDate < DateTimeOffset.UtcNow)
            {
                throw new FoosballGameAlreadyFinishedException();
            }
            
            var sets = game.Score.Sets;
            var currentSet = sets.FirstOrDefault(x => x.FinishedSet == null);
            if (currentSet == default)
            {
                currentSet = new FoosballGameSet();   
                game.Score.Sets.Add(currentSet);
            }
            switch (scoringTeam)
            {
                case ScoringTeam.TeamA:
                    currentSet.TeamAScore++;
                    break;
                case ScoringTeam.TeamB:
                    currentSet.TeamBScore++;
                    break;
            }

            if (currentSet.TeamAScore == maxScore || currentSet.TeamBScore == maxScore)
            {
                currentSet.FinishedSet = new FinishedFoosballGameSet();
            }
            
            var teamAFinishedSets =
                sets
                    .Where(x => x.FinishedSet != null)
                    .Where(x => x.TeamAScore > x.TeamBScore)
                    .ToList();
            var teamBFinishedSets =
                sets
                    .Where(x => x.FinishedSet != null)
                    .Where(x => x.TeamAScore < x.TeamBScore)
                    .ToList();
            if (teamAFinishedSets.Count == maxSetCount || teamBFinishedSets.Count == maxSetCount)
            {
                game.EndDate = DateTimeOffset.UtcNow;
            }

            _context.SaveChanges();
            return new GameStatus
            {
                GameId = game.Id,
                IsFinished = game.EndDate.HasValue
            };
        }

        public PlayerStatsModel GetPlayerStats(int playerId)
        {
            var playerGames =
                _context
                    .FoosballGames
                    .AsNoTracking()
                    .Where(x => x.EndDate != null)
                    .Where(
                        x => x.FoosballPlayerSetup.TeamA.FoosballTeamPlayers.Any(p => p.FoosballPlayerId == playerId)
                             || x.FoosballPlayerSetup.TeamB.FoosballTeamPlayers.Any(p => p.FoosballPlayerId == playerId)
                    )
                    .Select(x => new FoosballGameDataModel
                    {
                        Id = x.Id,
                        TeamAPlayerIds = x.FoosballPlayerSetup.TeamA.FoosballTeamPlayers.Select(x => x.FoosballPlayerId).ToList(),
                        TeamBPlayerIds = x.FoosballPlayerSetup.TeamB.FoosballTeamPlayers.Select(x => x.FoosballPlayerId).ToList(),
                        Sets = x.Score.Sets.Select(s => new FoosballGameSetModel
                        {
                            TeamAScore = s.TeamAScore,
                            TeamBScore = s.TeamBScore,
                            IsFinished = s.FinishedSet != null
                        }).ToList(),
                        StartDate = x.StartDate,
                        EndDate =  x.EndDate.Value
                    })
                    .ToList();

            var gamesWonInTeamA =
                playerGames
                    .Where(x => x.TeamAPlayerIds.Contains(playerId))
                    .Where(x => x.Sets.Where(s => s.TeamAScore > s.TeamBScore).Count() > x.Sets.Count / 2)
                    .Count();
            var gamesWonInTeamB =
                playerGames
                    .Where(x => x.TeamBPlayerIds.Contains(playerId))
                    .Where(x => x.Sets.Where(s => s.TeamAScore < s.TeamBScore).Count() > x.Sets.Count / 2)
                    .Count();
            var allGamesWon = gamesWonInTeamA + gamesWonInTeamB;
            
            return new PlayerStatsModel
            {
                GamesLost = playerGames.Count - allGamesWon,
                GamesWon = allGamesWon
            };
        }

        public FoosballGameDataModel[] GetGames()
        {
            return 
                GetFoosballGameDataModelQuery()
                    .OrderByDescending(x => x.StartDate)
                    .ToArray();
        }
        
        public FoosballGameDataModel GetGame(int gameId)
        {
            return
                GetFoosballGameDataModelQuery()
                    .First(x => x.Id == gameId);
        }

        private IQueryable<FoosballGameDataModel> GetFoosballGameDataModelQuery()
        {
            return _context
                .FoosballGames
                .AsNoTracking()
                .Select(x => new FoosballGameDataModel
                {
                    Id = x.Id,
                    TableId = x.TableId,
                    TeamAPlayerIds = x.FoosballPlayerSetup.TeamA.FoosballTeamPlayers.Select(x => x.FoosballPlayerId)
                        .ToList(),
                    TeamBPlayerIds = x.FoosballPlayerSetup.TeamB.FoosballTeamPlayers.Select(x => x.FoosballPlayerId)
                        .ToList(),
                    Sets = x.Score.Sets.Select(s => new FoosballGameSetModel
                    {
                        TeamAScore = s.TeamAScore,
                        TeamBScore = s.TeamBScore,
                        IsFinished = s.FinishedSet != null
                    }).ToList(),
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                });
        }
    }
}