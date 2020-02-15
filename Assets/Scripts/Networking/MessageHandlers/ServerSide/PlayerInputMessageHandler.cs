using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMessageHandler : BaseHandler<PlayerInputMessage>
{
	private PlayerRegistry _playerRegistry;
	private Server _server;
	private GameLoop _loop;

	public PlayerInputMessageHandler(
		MessageProcessor messageProcessor,
		PlayerRegistry playerRegistry,
		Server server,
		GameLoop loop
	) : base(messageProcessor)
	{
		_playerRegistry = playerRegistry;
		_server = server;
		_loop = loop;
	}

	// TODO: Client tick vs server tick. Send info to client about offset.
	public override void Handle(PlayerInputMessage message)
	{
		Player player = _playerRegistry.GetPlayerByUserName(message.Sender.ToString());
		player.Input = message.GetMovement();
		Debug.Log($"Client tick: {message.TickIndex}, server tick: {_loop.GetTickIndex()}");
		_server.SendServerTickMessage((int)message.TickIndex - (int)_loop.GetTickIndex(), message.TickIndex);

	}
}
