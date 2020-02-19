using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerClockMessageHandler : BaseHandler<ServerClockMessage>
{
	private GameLoop _loop;
	private Settings _settings;

	private RttHelper _rtt = new RttHelper(0, 0);

	public ServerClockMessageHandler(
		MessageProcessor messageProcessor,
		GameLoop loop,
		Settings settings
	) : base(messageProcessor)
	{
		_loop = loop;
		_settings = settings;
	}

	public override void Handle(ServerClockMessage message)
	{
		// Adjusts client tick rate to sync with server.
		if (_rtt.tick != message.TickIndex)
		{
			_rtt.tick = message.TickIndex;
			_rtt.value = (int)_loop.TickIndex - (int)message.clientSentTick;
		}

		int diff = message.tickOffset - _rtt.value / 2;
		Time.fixedDeltaTime = _loop.BaseTimeStep + _settings.clientClockAdjustmentCurve.Evaluate(diff);

		// TODO: temp below, for debugging
		_loop.clientToServerOffset = message.tickOffset;
		_loop.RTT = _rtt.value;
	}

	[Serializable]
	public class Settings
	{
		public AnimationCurve clientClockAdjustmentCurve;
	}

	public class RttHelper
	{
		public uint tick;
		public int value;

		public RttHelper(uint tick, int rtt)
		{
			this.tick = tick;
			this.value = rtt;
		}
	}
}
