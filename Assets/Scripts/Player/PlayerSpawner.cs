using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner
{
    private Player.Factory _factory;
	private Player.Settings _settings;

    public PlayerSpawner(Player.Factory factory, Player.Settings settings)
    {
        _factory = factory;
		_settings = settings;
	}

    public Player SpawnPlayer(string userName)
    {
		Player p = _factory.Create(_settings.speed);
		p.UserName = userName;

		return p;
    }
}
