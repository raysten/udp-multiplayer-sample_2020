using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallKick : IInitializable, IUpdatable
{
	private GameLoop _loop;
	private Ball _ball;
	private Settings _settings;

	private Dictionary<uint, List<InputData>> _inputs = new Dictionary<uint, List<InputData>>();

	public BallKick(GameLoop loop, Ball ball, Settings settings)
	{
		_loop = loop;
		_ball = ball;
		_settings = settings;
	}

	public void Initialize()
	{
		_loop.LateSubscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_inputs.TryGetValue(tickIndex, out var inputs))
		{
			foreach (InputData input in inputs)
			{
				ProcessInput(input);
			}

			_inputs.Remove(tickIndex);
		}
	}

	public void EnqueueInput(InputData input)
	{
		if (input.tickIndex >= _loop.TickIndex)
		{
			if (_inputs.ContainsKey(input.tickIndex))
			{
				_inputs[input.tickIndex].Add(input);
			}
			else
			{
				_inputs.Add(input.tickIndex, new List<InputData>() { input });
			}
		}
	}

	private void ProcessInput(InputData input)
	{
		var ballToPlayerVector = _ball.transform.position - input.playerTransform.position;

		if (ballToPlayerVector.sqrMagnitude < _settings.minSqrDistanceToPlayer)
		{
			_ball.Rigidbody.AddForce(ballToPlayerVector.normalized * _settings.force, ForceMode.Impulse);
		}
	}

	[System.Serializable]
	public class Settings
	{
		public float minSqrDistanceToPlayer = 3f;
		public float force = 7.5f;
	}

	public struct InputData
	{
		public uint tickIndex;
		public Transform playerTransform;

		public InputData(uint tickIndex, Transform playerTransform)
		{
			this.tickIndex = tickIndex;
			this.playerTransform = playerTransform;
		}
	}
}
