using System.Net;
using UnityEngine.UI;
using Zenject;

public class ConnectionGUI : IInitializable
{
	private InputField _inputField;
	private Button _connectionButton;
	private LocalClient _client;

	public ConnectionGUI(InputField inputField, Button connectionButton, LocalClient client)
	{
		_inputField = inputField;
		_connectionButton = connectionButton;
		_client = client;
	}

	public void Initialize()
	{
		_connectionButton.onClick.AddListener(Connect);
		_inputField.text = "192.168.8.110:54120";
	}

	// TODO: Error handling. Currently I assume correct ip and port.
	private void Connect()
	{
		string[] split = _inputField.text.Split(':');
		IPAddress ipAddress = IPAddress.Parse(split[0]);
		_client.SendHandshakeMessage(ipAddress, int.Parse(split[1]));
		HideGUI();
	}

	private void HideGUI()
	{
		_inputField.gameObject.SetActive(false);
		_connectionButton.gameObject.SetActive(false);
	}
}
