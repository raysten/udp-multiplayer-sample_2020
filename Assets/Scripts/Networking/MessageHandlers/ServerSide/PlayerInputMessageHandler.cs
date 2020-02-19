using System.Collections;
using System.Collections.Generic;

public class PlayerInputMessageHandler : BaseHandler<PlayerInputMessage>
{
	private PlayerRegistry _playerRegistry;
	private DebugScreen _debugScreen;

	public PlayerInputMessageHandler(
		MessageProcessor messageProcessor,
		PlayerRegistry playerRegistry,
		DebugScreen debugScreen
	) : base(messageProcessor)
	{
		_playerRegistry = playerRegistry;
		_debugScreen = debugScreen;
	}

	public override void Handle(PlayerInputMessage message)
	{
		Player player = _playerRegistry.GetPlayerByUserName(message.Sender.ToString());
		player.BufferInput(new InputData(message.GetMovement(), message.TickIndex));

	}
}
