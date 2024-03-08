namespace WMS.Core.Entities.Api.JukanV4Api.BaseObjects
{
    public class ItemsInfo
    {
        public int? Count { get; set; }
        public int? Total { get; set; }
        public int? per_page { get; set; }
    }

    public class PaginationInfo
    {
        public int? last_visible_page { get; set; }
        public bool has_next_page { get; set; }
        public int? current_page { get; set; }
        public ItemsInfo? Items { get; set; }
    }
}