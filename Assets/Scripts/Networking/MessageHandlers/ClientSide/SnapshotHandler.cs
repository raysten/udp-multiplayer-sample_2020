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
			Player remotePlayer = _playerRegistry.Players.Find(x => x.PlayerId == playerData.playerId);

			if (remotePlayer == null)
			{
				Player player = _playerRegistry.RegisterPlayer(_spawner.SpawnRemotePlayer(), playerData.playerId);
			}
			else if (remotePlayer.PlayerId != _localClient.LocalPlayerId)
			{
				// TODO: set player position properly with entity interpolation.
				remotePlayer.transform.position = playerData.position;
			}
		}
	}
}
