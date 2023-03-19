using Microsoft.AspNetCore.Mvc;

namespace StockTickerAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class StockTickerController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Symbol", "Price", "Volume", "Change", "ChangePercent", "DateTime"
        };

        private readonly ILogger<StockTickerController> _logger;

        public StockTickerController(ILogger<StockTickerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetStockTicker")]
        public IEnumerable<StockTicker> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new StockTicker
            {
                Symbol = "AAPL",
                Price = 146.92,
                Volume = 25927122,
                Change = -0.25,
                ChangePercent = -0.17,
                Date = DateTime.Now.AddDays(index),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
