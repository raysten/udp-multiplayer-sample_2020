using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IUpdatable
{
	[SerializeField]
	private CharacterController controller;

	[Inject]
	private float _speed;
	[Inject]
	private GameLoop _loop;
	private Queue<InputData> _inputBuffer = new Queue<InputData>();
	private Vector3 _input;

	public string UserName { get; set; }
	public int PlayerId { get; set; }

	public void Start()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
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

	private void Move(Vector3 motion)
	{
		float dt = Time.fixedDeltaTime;
		Vector3 movement = motion.normalized * dt * _speed;
		controller.Move(movement);
	}

	public class Factory : PlaceholderFactory<float, Player>
    {
    }

	[Serializable]
	public class Settings
	{
		public float speed = 2f;
	}
}
