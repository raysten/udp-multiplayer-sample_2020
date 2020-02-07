using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "ServerSettings", menuName = "Installers/ServerSettings")]
public class ServerSettings : ScriptableObjectInstaller<ServerSettings>
{
    [SerializeField]
    private Server.Settings serverConfiguration;


    public override void InstallBindings()
    {
        Container.BindInstance(serverConfiguration).AsSingle();
    }
}