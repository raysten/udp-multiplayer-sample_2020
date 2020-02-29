using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DebugScreen : MonoBehaviour
{
	[SerializeField]
	private Text textBox;
	[SerializeField]
	private Text extraBox;
	private GameLoop _loop;

    [Inject]
	public void Construct(GameLoop loop)
	{
		_loop = loop;
	}

	public void PrintExtraDebug(string text)
	{
		extraBox.text += text + "\n";
	}

	private void Update()
	{
		textBox.text = $"Tick: {_loop.TickIndex.ToString()}" +
			$", offset: {_loop.clientToServerOffset}" +
			$", dt: {_loop.TimeStep}" +
			$", rtt: {_loop.RTT}";
	}
}
