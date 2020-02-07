using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public abstract class BaseUdpMessage : IUdpMessage
{
    public IPEndPoint Sender { get; private set; }

    public virtual void Deserialize(IPEndPoint remote, DataReader reader)
    {
        Sender = remote;
    }

    public virtual void Serialize(DataWriter writer)
    {
    }
}
