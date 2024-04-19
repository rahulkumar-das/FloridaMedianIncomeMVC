namespace FloridaMedianIncomeMVC.Models
{
    public class CountyInfo
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<string>? Labels { get; set; }
        public List<int>? IncomeData { get; set; }
    }
}
