using System.Dynamic;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

LinkedList<WebSocket> clients = new();

app.MapGet("/", () => "Hello World!");

app.UseWebSockets();

app.Map("/ws/my-roomname", async (context) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var ws = await context.WebSockets.AcceptWebSocketAsync();
        clients.AddLast(ws);
        await Recieve(ws, clients);
    }
     else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();

async Task Recieve(WebSocket ws, ICollection<WebSocket>clients)
{
    var buffer = new byte[1024 * 64];
    
    try
    {
        while (ws.State == WebSocketState.Open)
        {
            List<Byte> messageBytes = new();
            WebSocketReceiveResult result;

            do
            {
                result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                messageBytes.AddRange(buffer.Take(result.Count));
            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                foreach (var client in clients.Where(c => c != ws && c.State == WebSocketState.Open))
                {
                    await client.SendAsync(messageBytes.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            }
        }
    } finally
    {
        clients.Remove(ws);
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }
}