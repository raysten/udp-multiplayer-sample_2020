using System.Net;

public interface IUdpMessage
{
    IPEndPoint Sender { get; }
	uint TickIndex { get; }

    void Deserialize(IPEndPoint remote, DataReader reader);
    void Serialize(DataWriter writer);
}