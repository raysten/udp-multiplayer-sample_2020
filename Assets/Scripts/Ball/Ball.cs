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
	private Settings _settings;

	private Queue<InputData> _buffer = new Queue<InputData>();
	private uint _lastReceivedTickIndex = 0;
	private uint _lastProcessedTick = 0;
	private int _minBufferedAmount = 3;

	public Rigidbody Rigidbody { get; private set; }

	[Inject]
	public void Construct(GameLoop loop, Settings settings)
	{
		_loop = loop;
		_settings = settings;
	}

	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();

		if (_isClient)
		{
			_loop.LateSubscribe(this);
		}
	}

	public void Simulate(uint tickIndex)
	{
		if (_buffer.Count >= _minBufferedAmount)
		{
			InputData currentInput = _buffer.Peek();

			if (_lastProcessedTick == 0)
			{
				_lastProcessedTick = currentInput.tickIndex - 1;
			}

			int diffToLastProcessedTick = (int)(currentInput.tickIndex - _lastProcessedTick);
			transform.position = Vector3.Lerp(transform.position, currentInput.position, 1f / diffToLastProcessedTick);

			// Don't dequeue input if we're not at its tick yet.
			if (diffToLastProcessedTick == 1)
			{
				_buffer.Dequeue();
			}

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
		if (_isClient == false && (other.tag == _settings.leftGoalTag || other.tag == _settings.rightGoalTag))
		{
			transform.position = new Vector3(0f, 3f, 0f);
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;
		}
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

	[System.Serializable]
	public class Settings
	{
		public string leftGoalTag;
		public string rightGoalTag;
	}
}
