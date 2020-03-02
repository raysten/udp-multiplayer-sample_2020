using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControlledPlayer : Player
{
	private Queue<InputData> _inputBuffer = new Queue<InputData>();
	private Vector3 _input;

	public override void Simulate(uint tickIndex)
	{
		if (_inputBuffer.Count > 0)
		{
			var firstInputInBuffer = _inputBuffer.Peek();

			if (firstInputInBuffer.tickIndex == tickIndex)
			{
				_inputBuffer.Dequeue();
				_input = firstInputInBuffer.input;
			}
			else if (firstInputInBuffer.tickIndex < tickIndex)
			{
				_inputBuffer.Dequeue();
			}
		}

		Move(_input);
		_input = Vector3.zero;
	}

	public void BufferInput(InputData inputData)
	{
		if (inputData.tickIndex >= _loop.TickIndex)
		{
			_inputBuffer.Enqueue(inputData);
		}
	}

	public void Move(Vector3 movement)
	{
		transform.position += movement * _settings.speed;
	}

	public class Factory : PlaceholderFactory<Team, ControlledPlayer>
    {
    }
}
