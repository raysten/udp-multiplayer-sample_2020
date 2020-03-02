using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner
{
    private ControlledPlayer.Factory _controlledPlayerFactory;
    private RemotePlayer.Factory _remotePlayerFactory;
	private Player.Settings _settings;

    public PlayerSpawner(
		ControlledPlayer.Factory controlledPlayerFactory,
		RemotePlayer.Factory remotePlayerFactory,
		Player.Settings settings
	)
    {
        _controlledPlayerFactory = controlledPlayerFactory;
		_remotePlayerFactory = remotePlayerFactory;
		_settings = settings;
	}

    public ControlledPlayer SpawnControlledPlayer(Team team)
    {
		return _controlledPlayerFactory.Create(team);
    }

	public RemotePlayer SpawnRemotePlayer(Team team)
	{
		return _remotePlayerFactory.Create(team);
	}
}
