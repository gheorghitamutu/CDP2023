namespace StockTickerAPI
{
    public class StockTicker
    {
        public string? Symbol { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public DateTime Date { get; set; }

        public string? Summary { get; set; }
    }
}
