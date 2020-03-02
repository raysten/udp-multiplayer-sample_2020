using System.Net;
using UnityEngine;

public class SpawnPlayerMessageHandler : BaseHandler<SpawnPlayerMessage>
{
    private PlayerSpawner _spawner;
	private GameLoop _loop;
	private LocalClient _client;
	private PlayerRegistry _playerRegistry;
	private EventBus _events;

    public SpawnPlayerMessageHandler(
        MessageProcessor messageProcessor,
        PlayerSpawner spawner,
		GameLoop loop,
		LocalClient client,
		PlayerRegistry playerRegistry,
		EventBus events
    ) : base(messageProcessor)
    {
        _spawner = spawner;
		_loop = loop;
		_client = client;
		_playerRegistry = playerRegistry;
		_events = events;
	}

    public override void Handle(SpawnPlayerMessage message)
    {
		if (_client.LocalPlayerId == -1)
		{
			// Client snaps to server's tick + some offset which will be adjusted later by clock sync.
			_loop.TickIndex = message.TickIndex + 5;
			_client.LocalPlayerId = message.playerId;
			_client.IsConnected = true;
			_client.SetServerEndpoint(message.Sender);
			_playerRegistry.RegisterPlayer(_spawner.SpawnControlledPlayer(message.team), message.playerId);
			_events.LocalPlayerSpawned?.Invoke();
		}
    }
}
