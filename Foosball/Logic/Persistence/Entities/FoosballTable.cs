using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Foosball.Logic.Persistence.Entities
{
    // represents a physical table
    public class FoosballTable
    {
        [Key]
        public int Id { get; set; }

        public virtual List<FoosballGame> FoosballGames { get; set; }
    }

    // mapping table

    // represents a single person - player

    // score of a game

    // represents a single set

    //
    // public class FinishedFoosballGameScore
    // {
    //     [Key]
    //     public int Id { get; set; }
    //     public int FoosballGameScoreId { get; set; }
    //     [ForeignKey("FoosballGameScoreId")]
    //     public FoosballGameScore Score { get; set; }
    //
    //     public DateTimeOffset EndDate { get; set; }
    // }
}