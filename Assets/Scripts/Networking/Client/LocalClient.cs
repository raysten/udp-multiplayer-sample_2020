using System.Net;
using Zenject;

public class LocalClient : IInitializable
{
	private UdpConnection _connection;
	private MessageSerializer _serializer;
	private MessageProcessor _messageProcessor;
	private PortFinder _portFinder;

	private IPEndPoint _serverEndpoint;

	public bool IsConnected { get; set; }
	public int LocalPlayerId { get; set; } = -1;

	public LocalClient(
		MessageSerializer serializer,
		MessageProcessor messageProcessor,
		PortFinder portFinder
	)
	{
		_serializer = serializer;
		_messageProcessor = messageProcessor;
		_portFinder = portFinder;
	}

	public void Initialize()
	{
		_connection = new UdpConnection(_portFinder.GetAvailablePort(54000));
	}

	public void SetServerEndpoint(IPAddress ip, int port)
	{
		_serverEndpoint = new IPEndPoint(ip, port);
	}

	public void SendMessage(IUdpMessage message)
	{
		var bytes = _serializer.SerializeMessage(message);
		_connection.Send(_serverEndpoint, bytes);
	}

	public void SendHandshakeMessage(IPAddress ip, int port)
	{
		SetServerEndpoint(ip, port);
		var message = new HandshakeMessage();
		var bytes = _serializer.SerializeMessage(message);
		_connection.Send(_serverEndpoint, bytes);
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
