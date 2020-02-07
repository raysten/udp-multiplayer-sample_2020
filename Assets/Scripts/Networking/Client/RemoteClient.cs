using System.Net;

public class RemoteClient
{
    private UdpConnection _connection;
    private MessageSerializer _serializer;
    private MessageProcessor _messageProcessor;

    public RemoteClient(MessageSerializer serializer, MessageProcessor messageProcessor)
    {
        _connection = new UdpConnection(54128);
        _serializer = serializer;
        _messageProcessor = messageProcessor;
    }

    public void SendWelcomeMessage(IPAddress ip, int port)
    {
        var welcome = new WelcomeMessage();
        var bytes = _serializer.SerializeMessage(welcome);
        _connection.Send(new IPEndPoint(ip, port), bytes);
        _connection.Listen(OnMessageReceived);
    }

    // TODO: timeout like in server
    private void OnMessageReceived(IPEndPoint endpoint, byte[] bytes)
    {
        var message = _serializer.ParseMessage(endpoint, bytes);

        if (message != null)
        {
            _messageProcessor.PushMessage(message);
        }

        _connection.Listen(OnMessageReceived);
    }
}
