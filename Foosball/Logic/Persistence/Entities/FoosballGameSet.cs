using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballGameSet
    {
        [Key]
        public int Id { get; set; }
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }

        public int FoosballGameScoreId { get; set; }
        [ForeignKey("FoosballGameScoreId")]
        public FoosballGameScore Score { get; set; }

        public FinishedFoosballGameSet FinishedSet { get; set; }
    }
}