//
// csc wsserver.cs
// wsserver.exe

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;

public static class Program
{
	public static void Main()
	{

		string sessionID = "";
		WebSocketServer webSocket = new WebSocketServer();
		webSocket.Listen(13000);
		webSocket.ClientConnected += (__, client) =>
		{
			sessionID = client.Id;
			client.AnyMessageReceived += (x, y) =>
			{
				var text = UTF8Encoding.UTF8.GetString(y);
				Console.WriteLine(text);
			};
		};
	tryagain:
		var a = Console.ReadLine();
		if (webSocket.Clients.Exists(X => X.Id == sessionID))
		{
			var c = webSocket.Clients.FirstOrDefault(X => X.Id == sessionID);
			var b = UTF8Encoding.UTF8.GetBytes(a);
			c.SendMessage(b, false, false);
		}
		goto tryagain;
	}
}