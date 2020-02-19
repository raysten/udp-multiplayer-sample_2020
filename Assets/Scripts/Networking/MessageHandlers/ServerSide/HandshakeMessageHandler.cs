using UnityEngine;

public class HandshakeMessageHandler : BaseHandler<HandshakeMessage>
{
	private PlayerSpawner _spawner;
	private Server _server;
	private PlayerRegistry _playerRegistry;

	public HandshakeMessageHandler(
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

	public override void Handle(HandshakeMessage message)
	{
		string clientId = message.Sender.ToString();

		if (!_server.HasClient(clientId))
		{
			Player player = _spawner.SpawnPlayer(clientId);
			_playerRegistry.RegisterPlayer(player);
			_server.RegisterClient(clientId, message.Sender);
			var spawnMessage = new SpawnPlayerMessage(clientId);
			_server.SendToAll(spawnMessage);
		}
		else
		{
			Debug.LogWarning("Client with this id is already connected.");
		}
	}
}
