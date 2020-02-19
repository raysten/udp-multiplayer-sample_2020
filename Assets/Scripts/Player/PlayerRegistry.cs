using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry
{
	private List<Player> _players = new List<Player>();

	public void RegisterPlayer(Player player)
	{
		_players.Add(player);
	}

	public Player GetPlayerByUserName(string userName)
	{
		return _players.Find(p => p.UserName == userName);
	}
}
