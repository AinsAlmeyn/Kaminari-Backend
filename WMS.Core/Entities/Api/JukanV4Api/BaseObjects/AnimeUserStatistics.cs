using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{

    public class AnimeUserStatistics
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public int watching { get; set; }
        public int completed { get; set; }
        public int on_hold { get; set; }
        public int dropped { get; set; }
        public int plan_to_watch { get; set; }
        public int total { get; set; }
        public List<Score> scores { get; set; }
    }

    public class Score
    {
        public int score { get; set; }
        public int votes { get; set; }
        public double percentage { get; set; }
    }

}