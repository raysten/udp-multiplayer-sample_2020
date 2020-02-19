using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerMessage : BaseUdpMessage
{
	public string playerName;

	public SpawnPlayerMessage()
	{
	}

	public SpawnPlayerMessage(string playerName)
	{
		this.playerName = playerName;
	}
}
