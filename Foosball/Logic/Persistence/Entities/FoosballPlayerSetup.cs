using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballPlayerSetup
    {
        [Key]
        public int Id { get; set; }
        
        public int FoosballGameId { get; set; }
        
        [ForeignKey("FoosballGameId")]
        public FoosballGame Game { get; set; }
        
        public FoosballTeam TeamA { get; set; }
        public FoosballTeam TeamB { get; set; }
    }
}