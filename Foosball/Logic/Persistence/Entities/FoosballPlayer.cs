using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballPlayer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<FoosballTeamPlayer> FoosballTeamPlayers { get; set; }
    }
}