using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SnapshotMessage : BaseUdpMessage
{
	public List<PlayerSnapshotData> playersData = new List<PlayerSnapshotData>();
	public Vector3 ballPosition;

	public SnapshotMessage()
	{
	}

	public SnapshotMessage(List<PlayerSnapshotData> playersData, Vector3 ballPosition)
	{
		this.playersData = playersData;
		this.ballPosition = ballPosition;
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(playersData.Count);

		for (int i = 0; i < playersData.Count; i++)
		{
			PlayerSnapshotData playerData = playersData[i];
			writer.Write(playerData.playerId);
			writer.Write(playerData.position);
			writer.Write((int)playerData.team);
		}

		writer.Write(ballPosition);
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		int playerAmount = reader.GetInteger();
		playersData.Clear();

		for (int i = 0; i < playerAmount; i++)
		{
			int playerId = reader.GetInteger();
			Vector3 playerPosition = reader.GetVector3();
			Team team = (Team)reader.GetInteger();
			PlayerSnapshotData playerData = new PlayerSnapshotData(playerId, playerPosition, team);
			playersData.Add(playerData);
		}

		ballPosition = reader.GetVector3();
	}
}

[Serializable]
public struct PlayerSnapshotData
{
	public int playerId;
	public Vector3 position;
	public uint tickIndex;
	public Team team;

	public PlayerSnapshotData(int playerId, Vector3 position, Team team)
	{
		this.playerId = playerId;
		this.position = position;
		this.team = team;
		this.tickIndex = 0;
	}
}
