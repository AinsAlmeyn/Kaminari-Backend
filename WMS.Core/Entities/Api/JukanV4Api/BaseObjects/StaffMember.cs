using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class StaffInfo
    {
        public List<StaffMember>? data { get; set; }
    }
    public class StaffMember
    {
        public Person? person { get; set; }
        public List<string>? positions { get; set; }
    }

    public class Person
    {
        public int? mal_id { get; set; }
        public string? url { get; set; }
        public ImageContainer? images { get; set; }
        public string? name { get; set; }
    }

    public class ImageContainer
    {
        public Image? jpg { get; set; }
    }

    public class Image
    {
        public string? image_url { get; set; }
    }

}