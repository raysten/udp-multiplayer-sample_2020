using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotHandler : BaseHandler<SnapshotMessage>
{
	private PlayerRegistry _playerRegistry;
	private LocalClient _localClient;
	private PlayerSpawner _spawner;

	public SnapshotHandler(
		MessageProcessor messageProcessor,
		PlayerRegistry playerRegistry,
		LocalClient localClient,
		PlayerSpawner spawner
	) : base(messageProcessor)
	{
		_playerRegistry = playerRegistry;
		_localClient = localClient;
		_spawner = spawner;
	}

	public override void Handle(SnapshotMessage message)
	{
		foreach (PlayerSnapshotData playerData in message.data)
		{
			Player player = _playerRegistry.Players.Find(x => x.PlayerId == playerData.playerId);

			if (player == null)
			{
				_playerRegistry.RegisterPlayer(_spawner.SpawnRemotePlayer(), playerData.playerId);
			}
			else if (player.PlayerId != _localClient.LocalPlayerId)
			{
				_playerRegistry.GetRemotePlayerById(player.PlayerId).EnqueuePosition(playerData, message.TickIndex);
			}
		}
	}
}
