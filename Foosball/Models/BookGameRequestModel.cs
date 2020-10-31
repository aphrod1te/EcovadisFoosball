using System;

namespace Foosball.Models
{
    public class BookGameRequestModel
    {
        public int? TableId { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public TeamSetup TeamSetup { get; set; }
    }
}