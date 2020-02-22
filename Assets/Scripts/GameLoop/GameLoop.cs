using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLoop : MonoBehaviour
{
	private DebugScreen _debugScreen;

	private List<IUpdatable> _subscribers = new List<IUpdatable>();
	private List<IUpdatable> _lateSubscribers = new List<IUpdatable>();

	// TODO: temp for debugging.
	public int clientToServerOffset;
	public int RTT;

	public uint TickIndex { get; set; }
	public float BaseTimeStep { get; private set; }

	[Inject]
	public void Construct(DebugScreen debugScreen)
	{
		_debugScreen = debugScreen;
	}

	private void Start()
	{
		BaseTimeStep = Time.fixedDeltaTime;
	}

	private void FixedUpdate()
	{
		Tick();
	}

	public void Subscribe(IUpdatable subscriber)
	{
		_subscribers.Add(subscriber);
	}

	public void LateSubscribe(IUpdatable subscriber)
	{
		_lateSubscribers.Add(subscriber);
	}

	private void Tick()
	{
		for (int i = 0; i < _subscribers.Count; i++)
		{
			_subscribers[i].Simulate(TickIndex);
		}

		for (int i = 0; i < _lateSubscribers.Count; i++)
		{
			_lateSubscribers[i].Simulate(TickIndex);
		}

		TickIndex++;
	}
}
