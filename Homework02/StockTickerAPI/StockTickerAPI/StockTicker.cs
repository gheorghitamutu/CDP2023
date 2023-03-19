using Newtonsoft.Json;

namespace StockTickerAPI
{
    public class StockTicker
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }
        
        [JsonProperty(PropertyName = "partitionKey")]
        public string? PartitionKey { get; set; }

        public string? Symbol { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
