namespace LaptopVendorRemake.Models
{
    public class FilterFormOptions
    {
        public int Brand { get; set; }
        public string OrderMode { get; set; }
        
        public bool FilterPrice { get; set; }
        public FilterPriceMode FilterPriceMode { get; set; }
        public double Price { get; set; }
        
        public bool FilterYear { get; set; }
        public FilterYearMode FilterYearMode { get; set; }
        public int Year { get; set; }
    }
}
