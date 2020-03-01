using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RemotePlayer : Player
{
	private Queue<PlayerSnapshotData> _buffer = new Queue<PlayerSnapshotData>();
	private uint _lastReceivedTickIndex = 0;
	private uint _lastProcessedTick = 0;
	private int _minBufferedAmount = 3;

	public override void Simulate(uint tickIndex)
	{
		if (_buffer.Count >= _minBufferedAmount)
		{
			PlayerSnapshotData currentInput = _buffer.Peek();

			if (_lastProcessedTick == 0)
			{
				_lastProcessedTick = currentInput.tickIndex - 1;
			}

			int diffToLastProcessedTick = (int)(currentInput.tickIndex - _lastProcessedTick);

			// Don't dequeue input if we're not at its tick yet.
			if (diffToLastProcessedTick == 1)
			{
				_buffer.Dequeue();
			}

			transform.position = Vector3.Lerp(transform.position, currentInput.position, 1f / diffToLastProcessedTick);
			_lastProcessedTick++;
		}
	}

	public void EnqueuePosition(PlayerSnapshotData inputData, uint tickIndex)
	{
		if (tickIndex > _lastReceivedTickIndex)
		{
			inputData.tickIndex = tickIndex;
			_buffer.Enqueue(inputData);
			_lastReceivedTickIndex = inputData.tickIndex;
		}
	}

	public class Factory : PlaceholderFactory<float, RemotePlayer>
	{
	}
}
