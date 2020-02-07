using System.Net;

public interface IUdpMessage
{
    IPEndPoint Sender { get; }

    void Deserialize(IPEndPoint remote, DataReader reader);
    void Serialize(DataWriter writer);
}