using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MessageProcessor : IInitializable, IUpdatable
{
	private GameLoop _loop;

    private Dictionary<Type, List<IMessageHandler>> _handlers = new Dictionary<Type, List<IMessageHandler>>();
	private Dictionary<uint, Queue<IUdpMessage>> _messages = new Dictionary<uint, Queue<IUdpMessage>>();
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

		//if (_messages.TryGetValue(tickIndex, out Queue<IUdpMessage> buffer))
		//{
		//	for (int i = 0; i < buffer.Count; i++)
		//	{
		//		IUdpMessage message = buffer[i];

		//		if (_handlers.TryGetValue(message.GetType(), out List<IMessageHandler> handlers))
		//		{
		//			foreach (var r in handlers)
		//			{
		//				r.Handle(message);
		//			}
		//		}
		//	}
		//}

		if (_messages.TryGetValue(tickIndex, out Queue<IUdpMessage> buffer))
		{
			while (buffer.Count > 0)
			{
				IUdpMessage message = buffer.Dequeue();

				if (_handlers.TryGetValue(message.GetType(), out List<IMessageHandler> handlers))
				{
					foreach (var r in handlers)
					{
						r.Handle(message);
					}
				}
			}
			_messages.Remove(tickIndex);
		}
	}

    public void AddMessage(IUdpMessage message)
    {
		// TODO:
		if (message.GetType() != typeof(PlayerInputMessage) && message.GetType() != typeof(ServerTickMessage))
		{
			_tickIgnoringMessages.Push(message);
		}
		else
		{
			if (_messages.TryGetValue(message.TickIndex, out Queue<IUdpMessage> buffer))
			{
				// TODO: Allow only one PlayerInputMessage per client during one tick.
				buffer.Enqueue(message);
			}
			else
			{
				_messages.Add(message.TickIndex, new Queue<IUdpMessage>());
				_messages[message.TickIndex].Enqueue(message);
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
