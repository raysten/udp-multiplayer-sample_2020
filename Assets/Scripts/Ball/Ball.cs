using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour, IUpdatable
{
	[SerializeField]
	private bool _isClient;
	[SerializeField]
	private float _lerpSpeed = 3f;

	private GameLoop _loop;

	private Queue<InputData> _buffer = new Queue<InputData>();
	private uint _lastReceivedTickIndex = 0;
	private uint _lastProcessedTick = 0;
	private int _minBufferedAmount = 3;

	[Inject]
	public void Construct(GameLoop loop)
	{
		_loop = loop;
	}

	private void Start()
	{
		if (_isClient)
		{
			_loop.LateSubscribe(this);
		}
	}

	public void Simulate(uint tickIndex)
	{
		if (_buffer.Count >= _minBufferedAmount)
		{
			InputData currentInput = _buffer.Dequeue();

			if (_lastProcessedTick == 0)
			{
				_lastProcessedTick = currentInput.tickIndex - 1;
			}

			int diffToLastProcessedTick = (int)(currentInput.tickIndex - _lastProcessedTick);
			transform.position = Vector3.Lerp(transform.position, currentInput.position, 1 / diffToLastProcessedTick);
			_lastProcessedTick++;
		}
	}

	public void EnqueuePosition(Vector3 position, uint tickIndex)
	{
		if (tickIndex > _lastReceivedTickIndex)
		{
			_buffer.Enqueue(new InputData(tickIndex, position));
			_lastReceivedTickIndex = tickIndex;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// TODO: Detect goals.
	}

	public struct InputData
	{
		public uint tickIndex;
		public Vector3 position;

		public InputData(uint tickIndex, Vector3 position)
		{
			this.tickIndex = tickIndex;
			this.position = position;
		}
	}
}
