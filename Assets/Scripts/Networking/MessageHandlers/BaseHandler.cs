using Zenject;

public abstract class BaseHandler<T> : IMessageHandler<T>, IInitializable where T : class, IUdpMessage
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

	public abstract void Handle(T message);

	public void Handle(IUdpMessage message)
	{
		Handle(message as T);
	}
}
