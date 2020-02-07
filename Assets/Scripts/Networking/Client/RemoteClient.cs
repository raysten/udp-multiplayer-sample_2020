using System.Net;
using Zenject;

public class RemoteClient : IInitializable
{
	private UdpConnection _connection;
	private MessageSerializer _serializer;
	private MessageProcessor _messageProcessor;
	private PortFinder _portFinder;

	public RemoteClient(MessageSerializer serializer, MessageProcessor messageProcessor, PortFinder portFinder)
	{
		_serializer = serializer;
		_messageProcessor = messageProcessor;
		_portFinder = portFinder;
	}

	public void Initialize()
	{
		_connection = new UdpConnection(_portFinder.GetAvailablePort(54000));
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
