using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MessageProcessor : IInitializable, IUpdatable
{
	private GameLoop _loop;

    private Dictionary<Type, List<IMessageHandler>> _handlers = new Dictionary<Type, List<IMessageHandler>>();
	private Dictionary<uint, List<IUdpMessage>> _messages = new Dictionary<uint, List<IUdpMessage>>();
	private Stack<IUdpMessage> _tickIgnoringMessages = new Stack<IUdpMessage>();

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
		while (_tickIgnoringMessages.Count > 0)
		{
			var message = _tickIgnoringMessages.Pop();
			List<IMessageHandler> handlers1 = null;

			if (_handlers.TryGetValue(message.GetType(), out handlers1))
			{
				foreach (var r in handlers1)
				{
					r.Handle(message);
				}
			}
		}

		if (_messages.TryGetValue(tickIndex, out List<IUdpMessage> buffer))
		{
			for (int i = 0; i < buffer.Count; i++)
			{
				IUdpMessage message = buffer[i];

				if (_handlers.TryGetValue(message.GetType(), out List<IMessageHandler> handlers))
				{
					foreach (var r in handlers)
					{
						r.Handle(message);
					}
				}
			}
		}
	}

    public void AddMessage(IUdpMessage message)
    {
		// TODO:
		if (message.GetType() != typeof(PlayerInputMessage))
		{
			_tickIgnoringMessages.Push(message);
		}
		else
		{
			if (_messages.TryGetValue(message.TickIndex, out List<IUdpMessage> buffer))
			{
				// TODO: Allow only one PlayerInputMessage per client during one tick.
				buffer.Add(message);
			}
			else
			{
				_messages.Add(message.TickIndex, new List<IUdpMessage>() { message });
			}
		}
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
