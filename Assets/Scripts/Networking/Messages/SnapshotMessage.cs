using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SnapshotMessage : BaseUdpMessage
{
	public List<PlayerSnapshotData> data = new List<PlayerSnapshotData>();

	public SnapshotMessage()
	{
	}

	public SnapshotMessage(List<PlayerSnapshotData> data)
	{
		this.data = data;
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(data.Count);

		for (int i = 0; i < data.Count; i++)
		{
			PlayerSnapshotData playerData = data[i];
			writer.Write(playerData.playerId);
			writer.Write(playerData.position);
		}
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		int playerAmount = reader.GetInteger();
		data.Clear();

		for (int i = 0; i < playerAmount; i++)
		{
			int playerId = reader.GetInteger();
			Vector3 playerPosition = reader.GetVector3();
			PlayerSnapshotData playerData = new PlayerSnapshotData(playerId, playerPosition);
			data.Add(playerData);
		}
	}
}

[Serializable]
public struct PlayerSnapshotData
{
	public int playerId;
	public Vector3 position;

	public PlayerSnapshotData(int playerId, Vector3 position)
	{
		this.playerId = playerId;
		this.position = position;
	}
}
