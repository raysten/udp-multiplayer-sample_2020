using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class BaseUdpMessage : IUdpMessage
{
    public IPEndPoint Sender { get; private set; }
	public uint TickIndex { get; private set; }

    public virtual void Deserialize(IPEndPoint remote, DataReader reader)
    {
        Sender = remote;
		TickIndex = reader.GetUnsignedInteger();
    }

    public virtual void Serialize(DataWriter writer)
    {
    }
}
