using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLoop : MonoBehaviour
{
	private Settings _settings;

	private uint _tickIndex;
	private List<IUpdatable> _subscribers = new List<IUpdatable>();
	private WaitForSeconds _waitTickDelta = null;
	private float _baseTimeStep;
	private float _deltaTimeEpsilon = 0.002f;

	// TODO: temp
	public int clientToServerOffset;

	[Inject]
	public void Construct(Settings settings)
	{
		_settings = settings;
	}

	private void Start()
	{
		_baseTimeStep = Time.fixedDeltaTime;
	}

	private void FixedUpdate()
	{
		Tick();
	}

	private IEnumerator Loop()
	{
		while(true)
		{
			Tick();

			if (_waitTickDelta == null)
			{
				_waitTickDelta = new WaitForSeconds(_settings.simulationRate);
			}

			yield return _waitTickDelta;
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

	public void HandleTickOffset(int tickOffset, uint clientSentTick)
	{
		int rtt = (int)_tickIndex - (int)clientSentTick;
		int diff = tickOffset - rtt / 2;
		clientToServerOffset = tickOffset;

		if (diff > 2)
		{
			Time.fixedDeltaTime = _baseTimeStep + _deltaTimeEpsilon;
		}
		else if (diff < -2)
		{
			Time.fixedDeltaTime = _baseTimeStep - _deltaTimeEpsilon;
		}
	}

	private void Tick()
	{
		for (int i = 0; i < _subscribers.Count; i++)
		{
			_subscribers[i].Simulate(_tickIndex);
		}

		_tickIndex++;
	}

	[Serializable]
	public class Settings
	{
		public float simulationRate = 1f / 30f;
	}
}
