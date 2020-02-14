using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DebugScreen : MonoBehaviour
{
	[SerializeField]
	private Text textBox;
	private GameLoop _loop;

    [Inject]
	public void Construct(GameLoop loop)
	{
		_loop = loop;
	}

	private void Update()
	{
		textBox.text = $"Tick: {_loop.GetTickIndex().ToString()}";
	}
}
