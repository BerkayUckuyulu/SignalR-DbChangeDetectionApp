using System;
using Microsoft.AspNetCore.SignalR;

namespace DCDServer.Hubs
{
	public class SalesHub:Hub
	{
		public async Task SendMessage()
		{
			await Clients.All.SendAsync("receiveMessage", "Hello");
		}
	}
}

