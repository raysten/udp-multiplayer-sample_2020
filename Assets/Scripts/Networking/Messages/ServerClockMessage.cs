using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerClockMessage : BaseUdpMessage
{
	public int tickOffset;
	public uint clientSentTick;

	public ServerClockMessage()
	{
	}

	public ServerClockMessage(int tickOffset, uint clientSentTick)
	{
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
