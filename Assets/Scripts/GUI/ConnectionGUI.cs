using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConnectionGUI : IInitializable, IDisposable
{
	private InputField _inputField;
	private Button _connectionButton;
	private LocalClient _client;
	private EventBus _events;

	public ConnectionGUI(InputField inputField, Button connectionButton, LocalClient client, EventBus events)
	{
		_inputField = inputField;
		_connectionButton = connectionButton;
		_client = client;
		_events = events;
	}

	public void Initialize()
	{
		_connectionButton.onClick.AddListener(Connect);
		_inputField.text = "192.168.43.235:54120";

		_events.LocalPlayerSpawned += OnLocalPlayerSpawned;
	}

	public void Dispose()
	{
		_events.LocalPlayerSpawned -= OnLocalPlayerSpawned;
	}

	private void OnLocalPlayerSpawned()
	{
		HideGUI();
	}

	// TODO: Error handling. Currently I assume correct ip and port.
	private void Connect()
	{
		string[] split = _inputField.text.Split(':');
		IPAddress ipAddress = IPAddress.Parse(split[0]);
		_client.SendHandshakeMessage(ipAddress, int.Parse(split[1]));
	}

	private void HideGUI()
	{
		_inputField.gameObject.SetActive(false);
		_connectionButton.gameObject.SetActive(false);
	}
}
