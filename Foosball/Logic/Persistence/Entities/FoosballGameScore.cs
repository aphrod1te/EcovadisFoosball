using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foosball.Logic.Persistence.Entities
{
    public class FoosballGameScore
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public List<FoosballGameSet> Sets { get; set; }

        public FoosballGame Game { get; set; }

        // public FinishedFoosballGameScore FinishedFoosballGameScore { get; set; }
    }
}