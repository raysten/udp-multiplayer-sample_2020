using System;
using System.Net;

class ConnectedClient
{
    public IPEndPoint Remote { get; }
    public DateTime LastMessage { get; set; }
    public string Username { get; set; }

	private Player _player;

    public ConnectedClient(IPEndPoint remote, Player player)
    {
        Remote = remote;
        LastMessage = DateTime.Now;
        Username = remote.ToString();
		_player = player;
    }
}