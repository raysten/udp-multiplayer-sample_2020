public interface IMessageHandler
{
    void Handle(IUdpMessage message);
}

public interface IMessageHandler<T> : IMessageHandler where T : IUdpMessage
{
    void Handle(T message);
}