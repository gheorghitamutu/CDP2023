using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;

public static class StockTickerFunction
{
    // used in attributes:
    const string EVENT_HUB_CONNECTION_STRING_PLAIN = "Endpoint=sb://stock-ticker.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Q6Xjs7/o1044U2SH/RwKNB4UbcWuzPfe6+AEhAdW6uQ=";

    // used.. somewhere else?
    public readonly static Expression EVENT_HUB_CONNECTION_STRING = Expression.Constant(Environment.GetEnvironmentVariable("EventHub_stockticker_1679176237014"));
    public readonly static Expression BLOB_STORAGE_CONNECTION_STRING = Expression.Constant(Environment.GetEnvironmentVariable("BlobStorage_stockticker"));

    [FunctionName("SendStockDataToEventHubViaWebSockets")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/sendStockData")] HttpRequest req,
        [ServiceBus("StockTickerRealTime", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        log.LogInformation($"StockTickerFunction HTTP trigger function started {req}.");

        // Get the WebSocket connection
        WebSocket webSocket = await req.HttpContext.WebSockets.AcceptWebSocketAsync();
        log.LogInformation("WebSocket connection accepted.");

        // Read stock data from WebSocket
        while (webSocket.State == WebSocketState.Open)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var data = JObject.Parse(Encoding.UTF8.GetString(buffer, 0, result.Count));

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
            }
        }

        return new OkResult();
    }


    [FunctionName("GenerateStockDataToEventHub")]
    public static async Task<IActionResult> Run([TimerTrigger("0 */30 * * * *")] TimerInfo _,
        [ServiceBus("StockTickerRealTime", Connection = EVENT_HUB_CONNECTION_STRING_PLAIN)] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        Console.WriteLine($"C# Timer trigger function executed at: {DateTime.Now}");

        List<StockTicker> stockData = GenerateStockData();

        foreach(var data in stockData)
        {
            var dataString = $"{data.Symbol},{data.Price},{data.Volume},{data.Change},{data.ChangePercent},{data.Date}";
            await eventHubMessages.AddAsync(dataString);
            log.LogInformation($"Stock data sent to Event Hub: {dataString}");
        }

        return new OkResult();
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
