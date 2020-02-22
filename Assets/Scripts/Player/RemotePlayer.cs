using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RemotePlayer : Player
{
	public override void Simulate(uint tickIndex)
	{
	}

	public class Factory : PlaceholderFactory<float, RemotePlayer>
	{
	}
}
