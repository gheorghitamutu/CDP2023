using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

public static class StockTickerFunction
{
    [FunctionName("StockTickerFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "StockTicker")] HttpRequest req,
        [ServiceBus("%EventHubName%", Connection = "EventHubConnection")] IAsyncCollector<string> eventHubMessages,
        ILogger log)
    {
        log.LogInformation("StockTickerFunction HTTP trigger function started.");

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
                var stockData = JObject.Parse(System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count));
                string stockSymbol = stockData["symbol"].ToString();
                double stockValue = double.Parse(stockData["value"].ToString());

                // Send stock data to Event Hub
                await eventHubMessages.AddAsync($"{stockSymbol},{stockValue}");
                log.LogInformation($"Stock data sent to Event Hub: {stockSymbol},{stockValue}");
            }
        }

        return new OkResult();
    }
}
