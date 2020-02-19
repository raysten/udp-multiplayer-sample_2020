using System.Net;
using UnityEngine;

public class SpawnPlayerMessageHandler : BaseHandler<SpawnPlayerMessage>
{
    private PlayerSpawner _spawner;
	private GameLoop _loop;
	private LocalClient _client;
	private PlayerRegistry _playerRegistry;

    public SpawnPlayerMessageHandler(
        MessageProcessor messageProcessor,
        PlayerSpawner spawner,
		GameLoop loop,
		LocalClient client,
		PlayerRegistry playerRegistry
    ) : base(messageProcessor)
    {
        _spawner = spawner;
		_loop = loop;
		_client = client;
		_playerRegistry = playerRegistry;
	}

    public override void Handle(SpawnPlayerMessage message)
    {
		// Client snaps to server's tick + some offset which will be adjusted later by clock sync.
		_loop.TickIndex = message.TickIndex + 5;

		if (_client.LocalPlayerId == -1)
		{
			_client.LocalPlayerId = message.playerId;
			_client.IsConnected = true;
		}

        Player player = _playerRegistry.RegisterPlayer(_spawner.SpawnPlayer(message.playerName), message.playerId);

		// TODO: refactor the hack
		if (player.PlayerId != _client.LocalPlayerId)
		{
			player.DisableMovementInUpdate = true;
		}
    }
}
