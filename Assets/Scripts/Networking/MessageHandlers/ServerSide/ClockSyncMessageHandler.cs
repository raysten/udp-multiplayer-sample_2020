using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSyncMessageHandler : BaseHandler<ClockSyncMessage>
{
	private Server _server;
	private GameLoop _loop;

	public ClockSyncMessageHandler(
		MessageProcessor messageProcessor,
		Server server,
		GameLoop loop
	) : base(messageProcessor)
	{
		_server = server;
		_loop = loop;
	}

	public override void Handle(ClockSyncMessage message)
	{
		var serverClockMessage = new ServerClockMessage((int)message.TickIndex - (int)_loop.TickIndex, message.TickIndex);
		_server.SendMessage(serverClockMessage, message.Sender);
	}
}
