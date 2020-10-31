using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballGame
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("TableId")]
        public FoosballTable Table { get; set; }
        public int TableId { get; set; }
        
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        // public int? FoosballPlayerSetupId { get; set; }
        // [ForeignKey("FoosballPlayerSetupId")]
        public FoosballPlayerSetup FoosballPlayerSetup { get; set; }

        public int FoosballGameScoreId { get; set; }
        [ForeignKey("FoosballGameScoreId")]
        public FoosballGameScore Score { get; set; } = new FoosballGameScore();
    }
}