using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLoop : ITickable
{
	private Settings _settings;

	private float _timeSinceLastTick;
	private uint _tickIndex;
	private List<IUpdatable> _subscribers = new List<IUpdatable>();

	public GameLoop(Settings settings)
	{
		_settings = settings;
	}

	public void Tick()
	{
		_timeSinceLastTick += Time.deltaTime;

		if (_timeSinceLastTick >= _settings.simulationRate)
		{
			for (int i = 0; i < _subscribers.Count; i++)
			{
				_subscribers[i].Simulate(_tickIndex);
			}

			_tickIndex++;
			_timeSinceLastTick = 0;
		}
	}

	public void Subscribe(IUpdatable subscriber)
	{
		_subscribers.Add(subscriber);
	}

	// TODO: convert tick index to property
	public uint GetTickIndex()
	{
		return _tickIndex;
	}

	public void SetTickIndex(uint value)
	{
		_tickIndex = value;
	}

	[Serializable]
	public class Settings
	{
		public float simulationRate = 1f / 30f;
	}
}
