using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballTeam
    {
        [Key]
        public int Id { get; set; }

        public List<FoosballTeamPlayer> FoosballTeamPlayers { get; set; }
    }
}