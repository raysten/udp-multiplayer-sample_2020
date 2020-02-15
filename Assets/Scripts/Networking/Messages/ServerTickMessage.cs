using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerTickMessage : BaseUdpMessage
{
	public int tickOffset;
	public uint clientSentTick;

	public ServerTickMessage()
	{
	}

	public ServerTickMessage(int tickOffset, uint clientSentTick)
	{
		// TODO:
		this.tickOffset = tickOffset;
		this.clientSentTick = clientSentTick;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		tickOffset = reader.GetInteger();
		clientSentTick = reader.GetUnsignedInteger();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(tickOffset);
		writer.Write(clientSentTick);
	}
}
