using System;
using System.Net;
using System.Net.Sockets;

public class UdpConnection
{
    private UdpClient _udpClient;
    private int _port;

    public UdpConnection(int port)
    {
        _port = port;
        _udpClient = new UdpClient(port);
    }

    public void Send(IPEndPoint remoteAddress, byte[] message)
    {
        _udpClient.Send(message, message.Length, remoteAddress);
    }

    public void Listen(Action<IPEndPoint, byte[]> action, int timeoutMs = 0)
    {
        var result = _udpClient.BeginReceive(new AsyncCallback(OnMessageReceived), action);

        if (timeoutMs > 0)
        {
            result.AsyncWaitHandle.WaitOne(timeoutMs);
        }
    }

    private void OnMessageReceived(IAsyncResult ar)
    {
        var action = (Action<IPEndPoint, byte[]>)ar.AsyncState;

        if (ar.IsCompleted)
        {
            IPEndPoint remoteIP = null;
            var bytes = _udpClient.EndReceive(ar, ref remoteIP);
            action?.Invoke(remoteIP, bytes);
        }
        else
        {
            action?.Invoke(null, null);
        }
    }
}
