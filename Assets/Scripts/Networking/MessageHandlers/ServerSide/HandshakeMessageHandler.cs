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
			ControlledPlayer player = _spawner.SpawnControlledPlayer(_playerRegistry.GetNextTeamAssignment());
			_playerRegistry.RegisterPlayer(player, clientId);
			_server.RegisterClient(clientId, message.Sender);
		}
		else
		{
			Debug.LogWarning("Client with this id is already connected.");
		}

		var serverPlayer = _playerRegistry.GetControlledPlayerByClientId(clientId);
		var spawnMessage = new SpawnPlayerMessage(clientId, serverPlayer.PlayerId, serverPlayer.Team);
		_server.SendMessage(spawnMessage, message.Sender);
	}
}
