using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServerInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject playerPrefab;
	[SerializeField]
	private GameObject remotePlayerPrefab;
	[SerializeField]
	private GameLoop gameLoop;
	[SerializeField]
	private DebugScreen debugScreen;
	[SerializeField]
	private Ball ball;

    public override void InstallBindings()
    {
		InstallGameLoop();
        InstallServer();
        InstallMessageScripts();
        InstallMessageHandlers();
        InstallSpawner();
        InstallPlayer();
		InstallBall();
		Container.BindInterfacesAndSelfTo<DebugScreen>().FromInstance(debugScreen).AsSingle();
    }

	private void InstallGameLoop()
	{
		Container.BindInterfacesAndSelfTo<GameLoop>().FromInstance(gameLoop).AsSingle();
	}

    private void InstallServer()
    {
        Container.BindInterfacesAndSelfTo<Server>().AsSingle();
		Container.BindInterfacesAndSelfTo<ServerSnapshotSystem>().AsSingle();
    }

    private void InstallMessageScripts()
    {
        Container.BindInterfacesAndSelfTo<MessageSerializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<MessageFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<MessageProcessor>().AsSingle();
    }

    private void InstallMessageHandlers()
    {
        Container.BindInterfacesAndSelfTo<HandshakeMessageHandler>().AsSingle();
		Container.BindInterfacesAndSelfTo<PlayerInputMessageHandler>().AsSingle();
		Container.BindInterfacesAndSelfTo<ClockSyncMessageHandler>().AsSingle();
    }

    private void InstallSpawner()
    {
        Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
    }

    private void InstallPlayer()
    {
        Container.BindFactory<float, ControlledPlayer, ControlledPlayer.Factory>().FromComponentInNewPrefab(playerPrefab);
        Container.BindFactory<float, RemotePlayer, RemotePlayer.Factory>().FromComponentInNewPrefab(remotePlayerPrefab);
		Container.BindInterfacesAndSelfTo<PlayerRegistry>().AsSingle();
    }

	private void InstallBall()
	{
		Container.BindInterfacesAndSelfTo<Ball>().FromInstance(ball).AsSingle();
	}
}
