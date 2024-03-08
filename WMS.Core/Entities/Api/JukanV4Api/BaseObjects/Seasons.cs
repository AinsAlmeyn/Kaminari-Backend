using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class Seasons
    {
        public List<SubSeason>? data { get; set; }
        public SubPagination? pagination { get; set; }
    }

    public class SubSeason
    {
        public int? year { get; set; }
        public List<string>? seasons { get; set; }
    }

    public class SubPagination
    {
        public int? last_visible_page { get; set; }
        public bool? has_next_page { get; set; }
    }

}