using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

public static class StockTickerFunction
{
    // used in attributes:
    const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";

    // used.. somewhere else?
    public readonly static Expression EVENT_HUB_CONNECTION_STRING = Expression.Constant(Environment.GetEnvironmentVariable("EventHub_stockticker_1679176237014"));
    public readonly static Expression BLOB_STORAGE_CONNECTION_STRING = Expression.Constant(Environment.GetEnvironmentVariable("BlobStorage_stockticker"));

    [FunctionName("SendStockDataToEventHub")]
    public static async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "/sendStockData")] HttpRequest req,
        [ServiceBus("StockTickerRealTime", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        log.LogInformation($"StockTickerFunction HTTP trigger function started {req}.");

        if (req.ContentType == "application/json")
        {
            var data = JObject.Parse(req.Body.ToString());

            string symbol = data["Symbol"].ToString();
            double price = double.Parse(data["Price"].ToString());
            int volume = int.Parse(data["Volume"].ToString());
            double change = double.Parse(data["Change"].ToString());
            double changePercent = double.Parse(data["ChangePercent"].ToString());
            DateTime date = DateTime.Parse(data["Date"].ToString());

            // Send stock data to Event Hub
            var dataString = $"{symbol},{price},{volume},{change},{changePercent},{date}";
            await eventHubMessages.AddAsync(dataString);
            log.LogInformation($"Stock data sent to Event Hub: {dataString}");

            return new Microsoft.AspNetCore.Mvc.OkResult();
        }

        return new Microsoft.AspNetCore.Mvc.BadRequestResult();
    }


    [FunctionName("GenerateStockDataToEventHub")]
    public static async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run([TimerTrigger("0 */30 * * * *")] TimerInfo _,
        [ServiceBus("StockTickerRealTime", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        Console.WriteLine($"C# Timer trigger function executed at: {DateTime.Now}");

        List<StockTicker> stockData = GenerateStockData();

        foreach (var data in stockData)
        {
            var dataString = $"{data.Symbol},{data.Price},{data.Volume},{data.Change},{data.ChangePercent},{data.Date}";
            await eventHubMessages.AddAsync(dataString);
            log.LogInformation($"Stock data sent to Event Hub: {dataString}");
        }

        return new Microsoft.AspNetCore.Mvc.OkResult();
    }

    private static List<StockTicker> GenerateStockData()
    {
        Random rnd = new();
        List<StockTicker> stockData = new();

        List<string> symbols = new() { "AAPL", "AMZN", "GOOGL", "TSLA", "MSFT", "NFLX", "NVDA", "PYPL", "FB", "JPM" };

        foreach (var symbol in symbols)
        {
            stockData.Add(new StockTicker()
            {
                Symbol = symbol,
                Price = rnd.Next(100, 1000),
                Volume = rnd.Next(100000, 1000000),
                Change = rnd.Next(-10, 10),
                ChangePercent = rnd.Next(-1000, 1000) / 100.0,
                Date = DateTime.UtcNow
            });
        }

        return stockData;
    }

    public class StockTicker
    {
        public string? Symbol { get; set; }

        public double Price { get; set; }

        public int Volume { get; set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public DateTime Date { get; set; }
    }
}
