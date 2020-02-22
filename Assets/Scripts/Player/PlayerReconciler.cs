using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerReconciler : IInitializable, IUpdatable
{
	private PlayerInputSystem _inputSystem;
	private GameLoop _loop;
	private LocalClient _client;
	private PlayerRegistry _playerRegistry;
	private Settings _settings;

	private Dictionary<uint, Vector3> _positionHistory = new Dictionary<uint, Vector3>();
	private uint _firstKeyOfPositionHistory = 0;
	private int _positionHistoryLength;
	private ControlledPlayer _localPlayer = null;

	public PlayerReconciler(
		PlayerInputSystem inputSystem,
		GameLoop loop,
		LocalClient client,
		PlayerRegistry playerRegistry,
		Settings settings
	)
	{
		_inputSystem = inputSystem;
		_loop = loop;
		_client = client;
		_playerRegistry = playerRegistry;
		_settings = settings;

		_positionHistoryLength = _settings.playerPositionHistoryLength;
	}

	public void Initialize()
	{
		_loop.LateSubscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_client.IsConnected)
		{
			if (_localPlayer == null)
			{
				_localPlayer = _playerRegistry.GetControlledPlayerById(_client.LocalPlayerId);
			}

			SavePositionHistory(_localPlayer.transform.position);
		}
	}

	public void Reconcile(uint serverTickIndex, Vector3 position)
	{
		if (_localPlayer == null)
		{
			return;
		}

		if (_positionHistory.TryGetValue(serverTickIndex, out Vector3 pastPosition) 
			&& (position - pastPosition).sqrMagnitude > _settings.positionDifferenceSqrEpsilon)
		{
			// TODO: remove
			//Debug.Log($"Reconciled; Server tick: {serverTickIndex}," +
			//	$" clientCurrentTick: {_loop.TickIndex}, " +
			//	$"position diff: {(position - pastPosition).magnitude}");

			_positionHistory[serverTickIndex] = position;
			_localPlayer.transform.position = position;

			// Replay subsequent inputs and rewrite position history.
			for (int i = (int)serverTickIndex + 1; i <= (int)_loop.TickIndex; i++)
			{
				if (_inputSystem.InputHistory.TryGetValue((uint)i, out Vector3 input))
				{
					_localPlayer.Move(input);

					if (_positionHistory.ContainsKey((uint)i))
					{
						_positionHistory[(uint)i] = _localPlayer.transform.position;
					}
				}
			}
		}
	}

	private void SavePositionHistory(Vector3 position)
	{
		if (_firstKeyOfPositionHistory == 0)
		{
			_firstKeyOfPositionHistory = _loop.TickIndex;
		}

		_positionHistory.Add(_loop.TickIndex, position);

		if (_positionHistory.Count > _positionHistoryLength)
		{
			_positionHistory.Remove(_firstKeyOfPositionHistory);
			_firstKeyOfPositionHistory++;
		}
	}

	[Serializable]
	public class Settings
	{
		public int playerPositionHistoryLength = 30;
		public float positionDifferenceSqrEpsilon = 0.002f;
	}
}
