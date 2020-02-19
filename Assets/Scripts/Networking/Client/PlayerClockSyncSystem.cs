using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerClockSyncSystem : IInitializable, IUpdatable
{
	private LocalClient _client;
	private GameLoop _loop;

	public PlayerClockSyncSystem(LocalClient client, GameLoop loop)
	{
		_client = client;
		_loop = loop;
	}

	public void Initialize()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_client.IsConnected)
		{
			var clockSyncMessage = new ClockSyncMessage();
			_client.SendMessage(clockSyncMessage);
		}
	}
}
