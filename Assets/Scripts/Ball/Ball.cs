using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// TODO: Reuse code for RemotePlayer as it's also interpolated entity.
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
	private int _maxBufferedAmount = 6; // TODO: Settings.

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
		if (_buffer.Count > 0)
		{
			InputData currentInput = _buffer.Dequeue();
			transform.position = currentInput.position;
		}
	}

	public void EnqueuePosition(Vector3 position, uint tickIndex)
	{
		if (tickIndex > _lastReceivedTickIndex)
		{
			int extraDiffToLastReceivedTickIndex = (int)(tickIndex - _lastReceivedTickIndex - 1);
			var freeBufferSpace = _maxBufferedAmount - _buffer.Count - 1;

			if (extraDiffToLastReceivedTickIndex > 0 && freeBufferSpace > 0)
			{
				int extraInterpolatedTicks = Mathf.Min(extraDiffToLastReceivedTickIndex, freeBufferSpace);

				for (int i = 0; i < extraInterpolatedTicks; i++)
				{
					_buffer.Enqueue(
						new InputData(
							(uint)(_lastReceivedTickIndex + i + 1),
							Vector3.Lerp(transform.position, position, (float)(1 + i) / extraInterpolatedTicks + 1)
						)
					);
				}
			}

			_buffer.Enqueue(new InputData(tickIndex, position));
			_lastReceivedTickIndex = tickIndex;
		}

		if (_buffer.Count > _maxBufferedAmount)
		{
			_buffer.Dequeue();
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
