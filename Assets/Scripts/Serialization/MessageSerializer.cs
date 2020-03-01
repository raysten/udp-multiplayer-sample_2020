using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class MessageSerializer
{
    private MessageFactory _messageFactory;
	private GameLoop _loop;

    private static readonly string HEADER_VERIFICATION = "chs";

    public MessageSerializer(MessageFactory messageFactory, GameLoop loop)
    {
        _messageFactory = messageFactory;
		_loop = loop;
	}

    public byte[] IsValid(byte[] data)
    {
        if (data.Length < HEADER_VERIFICATION.Length)
        {
            return null;
        }

        var sub = new byte[HEADER_VERIFICATION.Length];
        Array.Copy(data, 0, sub, 0, HEADER_VERIFICATION.Length);
        var head = Encoding.ASCII.GetString(sub);

        if (head == HEADER_VERIFICATION)
        {
            var split = new byte[data.Length - HEADER_VERIFICATION.Length];
            Array.Copy(data, HEADER_VERIFICATION.Length, split, 0, data.Length - HEADER_VERIFICATION.Length);

            return split;
        }

        return null;
    }

    public byte[] SerializeMessage(IUdpMessage message)
    {
        var writer = new DataWriter();
        writer.Write(HEADER_VERIFICATION, false);
        var cmdName = message.GetType().Name;
        writer.Write(cmdName);
		writer.Write(_loop.TickIndex);
        message.Serialize(writer);
        var bytes = writer.Finalize();

        return bytes;
    }

    public IUdpMessage ParseMessage(IPEndPoint remote, byte[] data)
    {
		if (remote == null || data == null)
		{
			return null;
		}

        var payloadData = IsValid(data);

        if (payloadData != null)
        {
            var reader = new DataReader(payloadData);
            var command = reader.GetString();

            var message = _messageFactory.CreateMessage(command, remote, reader);
            reader.Close();

            return message;
        }

        return null;
    }
}
