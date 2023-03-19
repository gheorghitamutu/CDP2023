using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public static class StockTickerFunction
{
    const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";


    [FunctionName("GenerateStockDataToEventHub")]
    public static async Task<Microsoft.AspNetCore.Mvc.IActionResult> Run([TimerTrigger("0 */30 * * * *")] TimerInfo _,
        [ServiceBus("StockTickerData", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
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
