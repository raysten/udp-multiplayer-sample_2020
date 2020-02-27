using System.Collections;
using System.Collections.Generic;

public class PlayerInputMessageHandler : BaseHandler<PlayerInputMessage>
{
	private PlayerRegistry _playerRegistry;
	private BallKick _ballKick;
	private DebugScreen _debugScreen;

	public PlayerInputMessageHandler(
		MessageProcessor messageProcessor,
		PlayerRegistry playerRegistry,
		BallKick ballKick,
		DebugScreen debugScreen
	) : base(messageProcessor)
	{
		_playerRegistry = playerRegistry;
		_ballKick = ballKick;
		_debugScreen = debugScreen;
	}

	public override void Handle(PlayerInputMessage message)
	{
		ControlledPlayer player = _playerRegistry.GetControlledPlayerById(message.PlayerId);
		player.BufferInput(new InputData(message.movement, message.TickIndex));

		if (message.kick)
		{
			_ballKick.EnqueueInput(new BallKick.InputData(message.TickIndex, player.transform));
		}
	}
}
