using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry
{
	private List<Player> _players = new List<Player>();

	public void RegisterPlayer(Player player)
	{
		_players.Add(player);
		player.PlayerId = _players.Count;
	}

	public void RegisterPlayer(Player player, int id)
	{
		_players.Add(player);
		player.PlayerId = id;
	}

	public Player GetPlayerByUserName(string userName)
	{
		return _players.Find(p => p.UserName == userName);
	}

	public Player GetPlayerById(int id)
	{
		return _players.Find(p => p.PlayerId == id);
	}
}
