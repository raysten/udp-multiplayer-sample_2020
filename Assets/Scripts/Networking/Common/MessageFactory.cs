using System;
using System.Collections.Generic;
using System.Net;

public class MessageFactory
{
    private Dictionary<string, Type> _messageTypes = new Dictionary<string, Type>();

    public MessageFactory()
    {
        foreach (var type in GetType().Assembly.GetTypes())
        {
            if (!type.IsAbstract && !type.IsInterface && type.GetInterface(typeof(IUdpMessage).Name) != null)
            {
                _messageTypes[type.Name] = type;
            }
        }
    }

    public IUdpMessage CreateMessage(string messageName, IPEndPoint remote, DataReader reader)
    {
        Type messageType = null;

        if (_messageTypes.TryGetValue(messageName, out messageType))
        {
            var message = (IUdpMessage)Activator.CreateInstance(messageType);
            message.Deserialize(remote, reader);

            return message;
        }

        return null;
    }
}