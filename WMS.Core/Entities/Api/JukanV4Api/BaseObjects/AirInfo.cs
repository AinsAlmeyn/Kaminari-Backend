namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class AirInfo
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public Prop? Prop { get; set; }
        public string? String { get; set; }
    }
    
    public class DateInfo
    {
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public class Prop
    {
        public DateInfo? From { get; set; }
        public DateInfo? To { get; set; }
    }
}