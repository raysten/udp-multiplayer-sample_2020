using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MessageProcessor : IFixedTickable
{
    private Dictionary<Type, List<IMessageHandler>> _handlers = new Dictionary<Type, List<IMessageHandler>>();

    private Stack<IUdpMessage> _messages = new Stack<IUdpMessage>();

    public void FixedTick()
    {
        while (_messages.Count > 0)
        {
            var message = _messages.Pop();
            List<IMessageHandler> handlers = null;

            if (_handlers.TryGetValue(message.GetType(), out handlers))
            {
                foreach (var r in handlers)
                {
                    r.Handle(message);
                }
            }
        }
    }

    public void PushMessage(IUdpMessage message)
    {
        _messages.Push(message);
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
