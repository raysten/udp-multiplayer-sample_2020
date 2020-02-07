using Zenject;

public abstract class BaseHandler<T> : IMessageHandler, IInitializable where T : IUdpMessage
{
    private MessageProcessor _messageProcessor;

    public BaseHandler(MessageProcessor messageProcessor)
    {
        _messageProcessor = messageProcessor;
    }

    public void Initialize()
    {
        _messageProcessor.Register(typeof(T), this);
    }

    public abstract void Handle(IUdpMessage message);
}
