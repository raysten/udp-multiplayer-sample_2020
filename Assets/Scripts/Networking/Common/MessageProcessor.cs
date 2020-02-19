using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MessageProcessor : IInitializable, IUpdatable
{
	private GameLoop _loop;

    private Dictionary<Type, List<IMessageHandler>> _handlers = new Dictionary<Type, List<IMessageHandler>>();
	private List<IUdpMessage> _messages = new List<IUdpMessage>();
	private List<IUdpMessage> _messagesCopy = new List<IUdpMessage>();

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
		_messagesCopy.Clear();
		_messagesCopy.AddRange(_messages);
		_messages.Clear();

		for (int i = 0; i < _messagesCopy.Count; i++)
		{
			IUdpMessage message = _messagesCopy[i];

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
		_messages.Add(message);
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
