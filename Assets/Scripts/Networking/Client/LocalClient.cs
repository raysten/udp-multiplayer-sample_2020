using System.Net;
using Zenject;

public class LocalClient : IInitializable
{
	private UdpConnection _connection;
	private MessageSerializer _serializer;
	private MessageProcessor _messageProcessor;
	private PortFinder _portFinder;
	private Server.Settings _settings;

	private IPEndPoint _serverEndpoint;

	public bool IsConnected { get; set; }
	public int LocalPlayerId { get; set; } = -1;

	public LocalClient(
		MessageSerializer serializer,
		MessageProcessor messageProcessor,
		PortFinder portFinder,
		Server.Settings settings
	)
	{
		_serializer = serializer;
		_messageProcessor = messageProcessor;
		_portFinder = portFinder;
		_settings = settings;
	}

	public void Initialize()
	{
		_connection = new UdpConnection(_portFinder.GetAvailablePort(_settings.port + 10));
	}

	public void SetServerEndpoint(IPAddress ip, int port)
	{
		_serverEndpoint = new IPEndPoint(ip, port);
	}

	public void SetServerEndpoint(IPEndPoint endpoint)
	{
		_serverEndpoint = endpoint;
	}

	public void SendMessage(IUdpMessage message)
	{
		var bytes = _serializer.SerializeMessage(message);
		_connection.Send(_serverEndpoint, bytes);
	}

	public void SendHandshakeMessage()
	{
		var message = new HandshakeMessage();
		var bytes = _serializer.SerializeMessage(message);
		_connection.Send(new IPEndPoint(IPAddress.Broadcast, _settings.port), bytes);
		_connection.Listen(OnMessageReceived);
	}

	private void OnMessageReceived(IPEndPoint endpoint, byte[] bytes)
	{
		var message = _serializer.ParseMessage(endpoint, bytes);

		if (message != null)
		{
			_messageProcessor.AddMessage(message);
		}

		_connection.Listen(OnMessageReceived);
	}
}
