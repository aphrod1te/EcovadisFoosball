using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foosball.Logic.Persistence.Entities
{
    public class FinishedFoosballGameSet
    {
        [Key]
        public int Id { get; set; }

        public int FoosballGameSetId { get; set; }
        [ForeignKey("FoosballGameSetId")]
        public FoosballGameSet Set { get; set; }
    }
}