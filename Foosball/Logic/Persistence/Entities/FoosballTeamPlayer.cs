using System.ComponentModel.DataAnnotations.Schema;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballTeamPlayer
    {
        public int FoosballTeamId { get; set; }
        [ForeignKey("FoosballTeamId")]
        public FoosballTeam FoosballTeam { get; set; }

        public int FoosballPlayerId { get; set; }
        [ForeignKey("FoosballPlayerId")]
        public FoosballPlayer FoosballPlayer { get; set; }
    }
}