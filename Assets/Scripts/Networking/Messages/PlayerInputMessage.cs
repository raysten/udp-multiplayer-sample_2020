using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerInputMessage : BaseUdpMessage
{
	public Vector3 movement;
	public bool kick;

	public int PlayerId { get; private set; }

	public PlayerInputMessage()
	{
	}

	public PlayerInputMessage(int playerId, Vector3 movement, bool kick)
	{
		PlayerId = playerId;
		this.movement = movement;
		this.kick = kick;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		PlayerId = reader.GetInteger();
		movement = reader.GetVector3();
		kick = reader.GetBool();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(PlayerId);
		writer.Write(movement);
		writer.Write(kick);
	}
}
