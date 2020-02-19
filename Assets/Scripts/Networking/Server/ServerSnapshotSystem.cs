using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServerSnapshotSystem : IInitializable, IUpdatable
{
	private GameLoop _loop;
	private PlayerRegistry _playerRegistry;
	private Server _server;

	private List<PlayerSnapshotData> _playersData = new List<PlayerSnapshotData>();

	public ServerSnapshotSystem(GameLoop loop, PlayerRegistry playerRegistry, Server server)
	{
		_loop = loop;
		_playerRegistry = playerRegistry;
		_server = server;
	}

	public void Initialize()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_playerRegistry.Players.Count == 0)
		{
			return;
		}

		_playersData.Clear();

		foreach (Player player in _playerRegistry.Players)
		{
			var playerData = new PlayerSnapshotData(player.PlayerId, player.transform.position);
			_playersData.Add(playerData);
		}

		var message = new SnapshotMessage(_playersData);
		_server.SendToAll(message);
	}
}
