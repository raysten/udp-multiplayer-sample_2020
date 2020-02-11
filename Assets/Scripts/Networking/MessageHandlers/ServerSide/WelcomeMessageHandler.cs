using UnityEngine;

public class WelcomeMessageHandler : BaseHandler<WelcomeMessage>
{
	private PlayerSpawner _spawner;
	private Server _server;
	private PlayerRegistry _playerRegistry;

	public WelcomeMessageHandler(
		MessageProcessor messageProcessor,
		PlayerSpawner spawner,
		Server server,
		PlayerRegistry playerRegistry
	) : base(messageProcessor)
	{
		_spawner = spawner;
		_server = server;
		_playerRegistry = playerRegistry;
	}

	public override void Handle(WelcomeMessage message)
	{
		string clientId = message.Sender.ToString();

		if (!_server.HasClient(clientId))
		{
			Player player = _spawner.SpawnPlayer(clientId);
			_playerRegistry.RegisterPlayer(player);
			_server.RegisterClient(clientId, message.Sender);
		}
		else
		{
			Debug.LogWarning("Client with this id is already connected.");
		}

		var spawnMessage = new SpawnPlayerMessage();
		_server.SendToAll(spawnMessage);
	}
}
