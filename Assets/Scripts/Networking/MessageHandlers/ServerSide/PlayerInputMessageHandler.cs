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

	// TODO: Allow only one PlayerInputMessage per client during one tick.
	public override void Handle(PlayerInputMessage message)
	{
		Player player = _playerRegistry.GetPlayerByUserName(message.Sender.ToString());
		player.Input = message.GetMovement();
	}
}
