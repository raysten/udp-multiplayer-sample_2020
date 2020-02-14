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

	[Inject]
	public void Construct(Settings settings)
	{
		_settings = settings;
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

	private void Tick()
	{
		for (int i = 0; i < _subscribers.Count; i++)
		{
			_subscribers[i].Simulate(_tickIndex);
		}

		_tickIndex++;
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
