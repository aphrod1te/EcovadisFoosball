using System;
using System.Collections.Generic;

namespace Foosball.Logic
{
    public class FoosballGameDataModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public List<FoosballGameSetModel> Sets { get; set; }
        public List<int> TeamAPlayerIds { get; set; }
        public List<int> TeamBPlayerIds { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}