using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTickMessageHandler : BaseHandler<ServerTickMessage>
{
	private GameLoop _loop;

	public ServerTickMessageHandler(
		MessageProcessor messageProcessor,
		GameLoop loop
	) : base(messageProcessor)
	{
		_loop = loop;
	}

	public override void Handle(ServerTickMessage message)
	{
		_loop.HandleTickOffset(message.tickOffset, message.clientSentTick);
	}
}
