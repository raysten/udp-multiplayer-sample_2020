using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerInputMessage : BaseUdpMessage
{
	public Vector3 movement;

	public int PlayerId { get; private set; }

	public PlayerInputMessage()
	{
	}

	public PlayerInputMessage(int playerId, Vector3 movement)
	{
		PlayerId = playerId;
		this.movement = movement;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		PlayerId = reader.GetInteger();
		movement = reader.GetVector3();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(PlayerId);
		writer.Write(movement);
	}
}
