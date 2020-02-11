using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClientInstaller : MonoInstaller
{
	[SerializeField]
	private InputField _inputField;
	[SerializeField]
	private Button _connectButton;
	[SerializeField]
	private GameObject playerPrefab;

	public override void InstallBindings()
	{
		InstallClient();
		InstallMessageScripts();
		InstallConnectionGUI();
		InstallMessageHandlers();
		InstallSpawner();
		InstallPlayer();
		InstallHelpers();
	}

	private void InstallClient()
	{
		Container.BindInterfacesAndSelfTo<RemoteClient>().AsSingle();
	}

	private void InstallMessageScripts()
	{
		Container.BindInterfacesAndSelfTo<MessageSerializer>().AsSingle();
		Container.BindInterfacesAndSelfTo<MessageFactory>().AsSingle();
		Container.BindInterfacesAndSelfTo<MessageProcessor>().AsSingle();
	}

	private void InstallConnectionGUI()
	{
		Container.BindInstance(_inputField).AsSingle();
		Container.BindInstance(_connectButton).AsSingle();
		Container.BindInterfacesAndSelfTo<ConnectionGUI>().AsSingle();
	}

	private void InstallMessageHandlers()
	{
		Container.BindInterfacesAndSelfTo<SpawnPlayerMessageHandler>().AsSingle();
	}

	private void InstallSpawner()
	{
		Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
	}

	private void InstallPlayer()
	{
		Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(playerPrefab);
		Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle();
	}

	private void InstallHelpers()
	{
		Container.BindInterfacesAndSelfTo<PortFinder>().AsSingle();
	}
}
