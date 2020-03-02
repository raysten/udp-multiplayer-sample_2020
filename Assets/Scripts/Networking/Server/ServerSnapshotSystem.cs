using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServerSnapshotSystem : IInitializable, IUpdatable
{
	private GameLoop _loop;
	private PlayerRegistry _playerRegistry;
	private Server _server;
	private Ball _ball;

	private List<PlayerSnapshotData> _playersData = new List<PlayerSnapshotData>();

	public ServerSnapshotSystem(
		GameLoop loop,
		PlayerRegistry playerRegistry,
		Server server,
		Ball ball
	)
	{
		_loop = loop;
		_playerRegistry = playerRegistry;
		_server = server;
		_ball = ball;
	}

	public void Initialize()
	{
		_loop.LateSubscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_playerRegistry.Players.Count == 0)
		{
			return;
		}

		_playersData.Clear();

		foreach (ControlledPlayer player in _playerRegistry.ControlledPlayers)
		{
			var playerData = new PlayerSnapshotData(player.PlayerId, player.transform.position, player.Team);
			_playersData.Add(playerData);
		}

		var message = new SnapshotMessage(_playersData, _ball.transform.position);
		_server.SendToAll(message);
	}
}
