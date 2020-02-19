using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry
{
	private List<Player> _players = new List<Player>();

	public List<Player> Players { get => _players; }

	public void RegisterPlayer(Player player)
	{
		_players.Add(player);
		player.PlayerId = _players.Count;
	}

	public Player RegisterPlayer(Player player, int id)
	{
		_players.Add(player);
		player.PlayerId = id;

		return player;
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
