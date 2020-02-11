using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMessageHandler : BaseHandler<PlayerInputMessage>
{
	private PlayerRegistry _playerRegistry;

	public PlayerInputMessageHandler(
		MessageProcessor messageProcessor,
		PlayerRegistry playerRegistry
	) : base(messageProcessor)
	{
		_playerRegistry = playerRegistry;
	}

	public override void Handle(PlayerInputMessage message)
	{
		Player player = _playerRegistry.GetPlayerByUserName(message.Sender.ToString());
		player.Move(message.GetMovement());
	}
}
