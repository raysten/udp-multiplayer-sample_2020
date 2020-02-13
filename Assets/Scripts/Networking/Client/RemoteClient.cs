using System.Net;
using Zenject;

public class RemoteClient : IInitializable
{
	private UdpConnection _connection;
	private MessageSerializer _serializer;
	private MessageProcessor _messageProcessor;
	private PortFinder _portFinder;
	private IPAddress _serverIp;
	private int _serverPort;

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

	// TODO: generic messaging
	public void SendWelcomeMessage(IPAddress ip, int port)
	{
		var welcome = new WelcomeMessage();
		var bytes = _serializer.SerializeMessage(welcome);
		_connection.Send(new IPEndPoint(ip, port), bytes);
		// TODO:
		_serverIp = ip;
		_serverPort = port;
		_connection.Listen(OnMessageReceived);
	}

	public void SendInputMessage(bool up, bool right, bool down, bool left)
	{
		var inputMessage = new PlayerInputMessage(up, right, down, left);
		var bytes = _serializer.SerializeMessage(inputMessage);
		_connection.Send(new IPEndPoint(_serverIp, _serverPort), bytes);
	}

	// TODO: timeout like in server
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
