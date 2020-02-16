using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MessageProcessor : IInitializable, IUpdatable
{
	private GameLoop _loop;

    private Dictionary<Type, List<IMessageHandler>> _handlers = new Dictionary<Type, List<IMessageHandler>>();
	private Queue<IUdpMessage> _messages = new Queue<IUdpMessage>();
	private Queue<IUdpMessage> _messagesSafe = new Queue<IUdpMessage>();

	public MessageProcessor(GameLoop loop)
	{
		_loop = loop;
	}

	public void Initialize()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
    {
		_messagesSafe.Clear();
		_messagesSafe = new Queue<IUdpMessage>(_messages); // TODO:
		_messages.Clear();

		while (_messagesSafe.Count > 0)
		{
			IUdpMessage message = _messagesSafe.Dequeue();

			if (_handlers.TryGetValue(message.GetType(), out List<IMessageHandler> handlers))
			{
				foreach (var r in handlers)
				{
					r.Handle(message);
				}
			}
		}
	}

    public void AddMessage(IUdpMessage message)
    {
		_messages.Enqueue(message);

		//if (_handlers.TryGetValue(message.GetType(), out List<IMessageHandler> handlers))
		//{
		//	foreach (var r in handlers)
		//	{
		//		r.Handle(message);
		//	}
		//}
	}

    public void Register(Type type, IMessageHandler handler)
    {
        List<IMessageHandler> handlers = null;

        if (!_handlers.TryGetValue(type, out handlers))
        {
            _handlers[type] = new List<IMessageHandler>();
            handlers = _handlers[type];
        }

        if (!handlers.Contains(handler))
        {
            handlers.Add(handler);
        }
    }
}
