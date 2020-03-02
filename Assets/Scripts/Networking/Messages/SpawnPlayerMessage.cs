using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpawnPlayerMessage : BaseUdpMessage
{
	public string playerName;
	public int playerId;
	public Team team;

	public SpawnPlayerMessage()
	{
	}

	public SpawnPlayerMessage(string playerName, int playerId, Team team)
	{
		this.playerName = playerName;
		this.playerId = playerId;
		this.team = team;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		playerName = reader.GetString();
		playerId = reader.GetInteger();
		team = (Team)reader.GetInteger();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(playerName);
		writer.Write(playerId);
		writer.Write((int)team);
	}
}
