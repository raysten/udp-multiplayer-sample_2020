using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputSystem : IInitializable, IUpdatable
{
	private LocalClient _client;
	private GameLoop _loop;
	private PlayerRegistry _playerRegistry;

	private Dictionary<uint, Vector3> _inputHistory = new Dictionary<uint, Vector3>();
	private uint _firstKeyOfInputHistory = 0;
	private int _inputHistoryLength = 30;
	private Player _localPlayer = null;

	public PlayerInputSystem(LocalClient client, GameLoop loop, PlayerRegistry playerRegistry)
	{
		_client = client;
		_loop = loop;
		_playerRegistry = playerRegistry;
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

			if (up || right || down || left)
			{
				var inputMessage = new PlayerInputMessage(up, right, down, left);
				Vector3 input = inputMessage.GetMovement();
				HandleInputHistory(input);
				_client.SendMessage(inputMessage);

				if (_localPlayer == null)
				{
					_localPlayer = _playerRegistry.GetPlayerById(_client.LocalPlayerId);
				}

				_localPlayer.BufferInput(new InputData(input, tickIndex + 1));
			}
		}
	}

	private void HandleInputHistory(Vector3 input)
	{
		if (_firstKeyOfInputHistory == 0)
		{
			_firstKeyOfInputHistory = _loop.TickIndex;
		}

		_inputHistory.Add(_loop.TickIndex, input);

		if (_inputHistory.Count > _inputHistoryLength)
		{
			_inputHistory.Remove(_firstKeyOfInputHistory);
			_firstKeyOfInputHistory++;
		}
	}
}
