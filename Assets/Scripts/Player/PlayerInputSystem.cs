using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputSystem : IInitializable, IUpdatable
{
	private LocalClient _client;
	private GameLoop _loop;
	private PlayerRegistry _playerRegistry;
	private PlayerReconciler.Settings _settings;

	public Dictionary<uint, Vector3> InputHistory { get; } = new Dictionary<uint, Vector3>();
	private uint _firstKeyOfInputHistory = 0;
	private int _inputHistoryLength;
	private ControlledPlayer _localPlayer = null;

	public PlayerInputSystem(
		LocalClient client,
		GameLoop loop,
		PlayerRegistry playerRegistry,
		PlayerReconciler.Settings settings
	)
	{
		_client = client;
		_loop = loop;
		_playerRegistry = playerRegistry;
		_settings = settings;

		_inputHistoryLength = _settings.playerPositionHistoryLength;
	}

	public void Initialize()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (_client.IsConnected)
		{
			bool up = Input.GetKey(KeyCode.W);
			bool right = Input.GetKey(KeyCode.D);
			bool down = Input.GetKey(KeyCode.S);
			bool left = Input.GetKey(KeyCode.A);
			bool space = Input.GetKeyDown(KeyCode.Space);

			if (up || right || down || left || space)
			{
				Vector3 input = GetMovement(up, right, down, left);
				HandleInputHistory(input);
				var inputMessage = new PlayerInputMessage(_client.LocalPlayerId, input, space);
				_client.SendMessage(inputMessage);

				if (_localPlayer == null)
				{
					_localPlayer = _playerRegistry.GetControlledPlayerById(_client.LocalPlayerId);
				}

				_localPlayer.BufferInput(new InputData(input, tickIndex));
			}
			else
			{
				HandleInputHistory(Vector3.zero);
			}
		}
	}

	private void HandleInputHistory(Vector3 input)
	{
		if (_firstKeyOfInputHistory == 0)
		{
			_firstKeyOfInputHistory = _loop.TickIndex;
		}

		InputHistory.Add(_loop.TickIndex, input);

		if (InputHistory.Count > _inputHistoryLength)
		{
			InputHistory.Remove(_firstKeyOfInputHistory);
			_firstKeyOfInputHistory++;
		}
	}

	private Vector3 GetMovement(bool up, bool right, bool down, bool left)
	{
		float xMovement = 0;
		float zMovement = 0;

		if (left)
		{
			xMovement += -1f;
		}

		if (right)
		{
			xMovement += 1f;
		}

		if (up)
		{
			zMovement += 1f;
		}

		if (down)
		{
			zMovement += -1f;
		}

		return new Vector3(xMovement, 0f, zMovement) * _loop.TimeStep;
	}
}
