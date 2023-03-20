using Microsoft.Extensions.Azure;
using System.Net;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.WebPubSub;
using Microsoft.Azure.WebPubSub.AspNetCore;
using Microsoft.Azure.WebPubSub.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddWebPubSub(
    o => o.ServiceEndpoint = new ServiceEndpoint("Endpoint=https://stockticker.webpubsub.azure.com;AccessKey=BLHVoRzD7RN5p9yM/qo0gMaZbbzJOLwaXQDk+KeOXds=;Version=1.0;"))
    .AddWebPubSubServiceClient<StockTicker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/negotiate", async (WebPubSubServiceClient<StockTicker> serviceClient, HttpContext context) =>
    {
        var id = context.Request.Query["id"];
        if (id.Count != 1)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("missing user id");
            return;
        }
        await context.Response.WriteAsync(serviceClient.GetClientAccessUri(userId: id).AbsoluteUri);
    });

    endpoints.MapWebPubSubHub<StockTicker>("/eventhandler/{*path}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

sealed class StockTicker : WebPubSubHub
{
    private readonly WebPubSubServiceClient<StockTicker> _serviceClient;

    public StockTicker(WebPubSubServiceClient<StockTicker> serviceClient)
    {
        _serviceClient = serviceClient;
    }

    public override async Task OnConnectedAsync(ConnectedEventRequest request)
    {
        await _serviceClient.SendToAllAsync($"[SYSTEM] {request.ConnectionContext.UserId} joined.");
    }

    public override async ValueTask<UserEventResponse> OnMessageReceivedAsync(UserEventRequest request, CancellationToken cancellationToken)
    {
        await _serviceClient.SendToAllAsync($"[{request.ConnectionContext.UserId}] {request.Data}");

        return request.CreateResponse($"[SYSTEM] ack.");
    }
}