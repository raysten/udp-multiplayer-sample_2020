using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInputSystem : ITickable
{
	private RemoteClient _client;

	public PlayerInputSystem(RemoteClient client)
	{
		_client = client;
	}

	public void Tick()
	{
		bool up = Input.GetKeyDown(KeyCode.W);
		bool right = Input.GetKeyDown(KeyCode.D);
		bool down = Input.GetKeyDown(KeyCode.S);
		bool left = Input.GetKeyDown(KeyCode.A);

		if (up || right || down || left)
		{
			_client.SendInputMessage(up, right, down, left);
		}
	}
}
