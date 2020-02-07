using System;
using System.Net;

class ConnectedClient
{
    public IPEndPoint Remote { get; }
    public DateTime LastMessage { get; set; }
    public string Username { get; set; }

    public ConnectedClient(IPEndPoint remote)
    {
        Remote = remote;
        LastMessage = DateTime.Now;
        Username = remote.ToString();
    }
}