using System;
using System.Linq;
using Foosball.Logic.Persistence;
using Foosball.Logic.Persistence.Entities;

namespace Foosball.Logic
{
    public class FoosballGameBuilder
    {
        private readonly FoosballContext _context;

        private int tableId;
        private DateTimeOffset startDate = DateTimeOffset.UtcNow;
        private FoosballTeam teamA;
        private FoosballTeam teamB;
        
        public FoosballGameBuilder(FoosballContext context, int tableId)
        {
            _context = context;
            this.tableId = tableId;
        }

        public FoosballGameBuilder SetStartDate(DateTimeOffset startDate)
        {
            this.startDate = startDate;
            return this;
        }

        public FoosballGameBuilder SetTeams(int[] teamAPlayerIds, int[] teamBPlayerIds)
        {
            var existingTeamA =
                _context
                    .FoosballTeams
                    .FirstOrDefault(x => x.FoosballTeamPlayers.All(p => teamAPlayerIds.Contains(p.FoosballPlayerId)));
            var existingTeamB = 
                _context
                    .FoosballTeams
                    .FirstOrDefault(x => x.FoosballTeamPlayers.All(p => teamBPlayerIds.Contains(p.FoosballPlayerId)));
            if (existingTeamA != default && existingTeamB != default)
            {
                teamA = existingTeamA;
                teamB = existingTeamB;
            }
            else
            {
                var playerList = teamAPlayerIds.Union(teamBPlayerIds).ToList();
                var players =
                    _context
                        .FoosballTeamPlayers
                        .Where(x => playerList.Contains(x.Id))
                        .ToList();
                teamA = new FoosballTeam();
                teamB = new FoosballTeam();
                var teamAplayers =
                   teamAPlayerIds 
                        .Select(x => new FoosballTeamPlayer
                            {
                                FoosballTeam =  teamA,
                                FoosballPlayer = players.First(p => p.Id == x)
                            }).ToList();
                var teamBplayers = 
                   teamBPlayerIds
                        .Select(x => new FoosballTeamPlayer
                            {
                                FoosballTeam =  teamB,
                                FoosballPlayer = players.First(p => p.Id == x)
                            }).ToList();
                teamA.FoosballTeamPlayers = teamAplayers;
                teamB.FoosballTeamPlayers = teamBplayers;
            }
            
            return this;
        }

        public FoosballGame Build()
        {
            var game = new FoosballGame
            {
                TableId = tableId,
                StartDate = startDate
            };

            if (teamA != default && teamB != default)
            {
                game.FoosballPlayerSetup = new FoosballPlayerSetup
                {
                    TeamA = teamA,
                    TeamB = teamB
                };
            }

            return game;
        }
    }
}