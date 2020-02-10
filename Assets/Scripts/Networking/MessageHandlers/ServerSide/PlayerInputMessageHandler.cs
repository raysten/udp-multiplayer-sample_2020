using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMessageHandler : BaseHandler<PlayerInputMessage>
{
	private Server _server;

	public PlayerInputMessageHandler(
		MessageProcessor messageProcessor,
		Server server
	) : base(messageProcessor)
	{
		_server = server;
	}

	public override void Handle(IUdpMessage message)
	{
		// TODO: Get ConnectedClient and call move on its Player.
	}
}
