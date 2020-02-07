using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServerInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject playerPrefab;

    public override void InstallBindings()
    {
        InstallServer();
        InstallMessageScripts();
        InstallMessageHandlers();
        InstallSpawner();
        InstallPlayer();
    }

    private void InstallServer()
    {
        Container.BindInterfacesAndSelfTo<Server>().AsSingle();
    }

    private void InstallMessageScripts()
    {
        Container.BindInterfacesAndSelfTo<MessageSerializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<MessageFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<MessageProcessor>().AsSingle();
    }

    private void InstallMessageHandlers()
    {
        Container.BindInterfacesAndSelfTo<WelcomeMessageHandler>().AsSingle();
    }

    private void InstallSpawner()
    {
        Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle();
    }

    private void InstallPlayer()
    {
        Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(playerPrefab);
    }
}
