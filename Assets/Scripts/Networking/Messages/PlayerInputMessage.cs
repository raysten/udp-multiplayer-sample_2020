using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerInputMessage : BaseUdpMessage
{
	private bool _up;
	private bool _right;
	private bool _down;
	private bool _left;

	public PlayerInputMessage()
	{
	}

	public PlayerInputMessage(bool up, bool right, bool down, bool left)
	{
		_up = up;
		_right = right;
		_down = down;
		_left = left;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		_up = reader.GetBool();
		_right = reader.GetBool();
		_down = reader.GetBool();
		_left = reader.GetBool();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(_up);
		writer.Write(_right);
		writer.Write(_down);
		writer.Write(_left);
	}

	public Vector3 GetMovement()
	{
		float xMovement = 0;
		float zMovement = 0;

		if (_left)
		{
			xMovement += -1f;
		}

		if (_right)
		{
			xMovement += 1f;
		}

		if (_up)
		{
			zMovement += 1f;
		}

		if (_down)
		{
			zMovement += -1f;
		}

		return new Vector3(xMovement, 0f, zMovement);
	}
}
