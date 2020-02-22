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
		//Debug.Log($"Remote player update, buffer size: {_buffer.Count}");
		if (_buffer.Count >= _minBufferedAmount)
		{
			PlayerSnapshotData currentInput = _buffer.Dequeue();

			if (_lastProcessedTick == 0)
			{
				_lastProcessedTick = currentInput.tickIndex - 1;
			}

			int diffToLastProcessedTick = (int)(currentInput.tickIndex - _lastProcessedTick);
			transform.position = Vector3.Lerp(transform.position, currentInput.position, 1 / diffToLastProcessedTick);
			_lastProcessedTick++;
		}
	}

	public void EnqueuePosition(PlayerSnapshotData inputData, uint tickIndex)
	{
		//Debug.Log("Enqueue remote player input");
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
