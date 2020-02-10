using UnityEngine;

public class WelcomeMessageHandler : BaseHandler<WelcomeMessage>
{
	private PlayerSpawner _spawner;
	private Server _server;

	public WelcomeMessageHandler(
		MessageProcessor messageProcessor,
		PlayerSpawner spawner,
		Server server
	) : base(messageProcessor)
	{
		_spawner = spawner;
		_server = server;
	}

	public override void Handle(IUdpMessage message)
	{
		string clientId = message.Sender.ToString();

		if (!_server.HasClient(clientId))
		{
			Player player = _spawner.SpawnPlayer();
			_server.RegisterClient(clientId, message.Sender, player);
		}
		else
		{
			Debug.LogWarning("Client with this id is already connected.");
		}

		var spawnMessage = new SpawnPlayerMessage();
		_server.SendToAll(spawnMessage);
	}
}
