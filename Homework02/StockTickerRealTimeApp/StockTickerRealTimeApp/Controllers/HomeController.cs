﻿using Microsoft.AspNetCore.Mvc;
using StockTickerRealTimeApp.Models;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Extensions.WebPubSub;
using Microsoft.Azure.WebPubSub.Common;
using Microsoft.Azure.WebJobs;
using Azure.Messaging.WebPubSub;
using System.Net.WebSockets;
using Websocket.Client;
using System.Text.Json;
using System;

namespace StockTickerRealTimeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        WebPubSubServiceClient _serviceClient;
        Uri _url;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _serviceClient = new WebPubSubServiceClient(
                "Endpoint=https://stockticker.webpubsub.azure.com;AccessKey=BLHVoRzD7RN5p9yM/qo0gMaZbbzJOLwaXQDk+KeOXds=;Version=1.0;", 
                "stocks");

            _url = _serviceClient.GetClientAccessUri(
                userId: "StockTickerRealTimeApp", 
                roles: new string[] { "webpubsub.joinLeaveGroup.stocks", "webpubsub.sendToGroup.stocks" });
            
            using (var client = new WebsocketClient(_url, () =>
            {
                var inner = new ClientWebSocket();
                inner.Options.AddSubProtocol("json.webpubsub.azure.v1");
                return inner;
            }))
            {
                // Disable the auto disconnect and reconnect because the sample would like the client to stay online even no data comes in
                client.ReconnectTimeout = null;
                client.MessageReceived.Subscribe(msg => Console.WriteLine($"Message received: {msg}"));
                client.Start();
                Console.WriteLine("Connected.");
                client.Send(JsonSerializer.Serialize(new
                {
                    type = "joinGroup",
                    group = "stocks",
                    ackId = 1
                }));
            }
        }

        public IActionResult Index()
        {
            ViewBag.Message = _url.ToString();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [FunctionName("connect")]
        public static WebPubSubEventResponse Connect(
            [WebPubSubTrigger("stocks", WebPubSubEventType.System, "connect")] ConnectEventRequest request)
        {
            Console.WriteLine($"Received client connect with connectionId: {request.ConnectionContext.ConnectionId}");
            return request.CreateResponse(request.ConnectionContext.UserId, null, null, null);
        }

        [FunctionName("disconnect")]
        [return: WebPubSub(Hub = "%WebPubSubHub%")]
        public static WebPubSubAction Disconnect(
            [WebPubSubTrigger("stocks", WebPubSubEventType.System, "disconnected")] WebPubSubConnectionContext connectionContext)
        {
            Console.WriteLine("Disconnect.");
            return new SendToAllAction
            {
                Data = BinaryData.FromString($"{connectionContext.UserId} disconnect."),
                DataType = WebPubSubDataType.Text
            };
        }
    }
}