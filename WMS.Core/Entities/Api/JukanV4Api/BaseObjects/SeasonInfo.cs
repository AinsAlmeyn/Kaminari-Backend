using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class SeasonInfo
    {
        public int? year { get; set; }
        public string? season { get; set; }
        public string? userId { get; set; }
    }
}