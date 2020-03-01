using System.Collections.Generic;
using UnityEngine;

public class RemoteEntity
{
	private Transform _transform;
	private Settings _settings;

	private Queue<InputData> _buffer = new Queue<InputData>();
	private uint _lastReceivedTickIndex = 0;

	public RemoteEntity(Transform transform, Settings settings)
	{
		_transform = transform;
		_settings = settings;
	}

	public void Simulate()
	{
		if (_buffer.Count > 0)
		{
			InputData currentInput = _buffer.Dequeue();
			_transform.position = currentInput.position;
		}
	}

	public void EnqueuePosition(Vector3 position, uint tickIndex)
	{
		if (tickIndex > _lastReceivedTickIndex)
		{
			int extraDiffToLastReceivedTickIndex = (int)(tickIndex - _lastReceivedTickIndex - 1);
			var freeBufferSpace = _settings.bufferSize - _buffer.Count - 1;

			if (extraDiffToLastReceivedTickIndex > 0 && freeBufferSpace > 0)
			{
				int extraInterpolatedTicks = Mathf.Min(extraDiffToLastReceivedTickIndex, freeBufferSpace);

				for (int i = 0; i < extraInterpolatedTicks; i++)
				{
					_buffer.Enqueue(
						new InputData(
							(uint)(_lastReceivedTickIndex + i + 1),
							Vector3.Lerp(_transform.position, position, (float)(1 + i) / extraInterpolatedTicks + 1)
						)
					);
				}
			}

			_buffer.Enqueue(new InputData(tickIndex, position));
			_lastReceivedTickIndex = tickIndex;
		}

		if (_buffer.Count > _settings.bufferSize)
		{
			_buffer.Dequeue();
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
		public int bufferSize = 6;
	}
}
