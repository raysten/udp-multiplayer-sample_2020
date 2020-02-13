using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "ServerSettings", menuName = "Installers/ServerSettings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField]
    private Server.Settings serverConfiguration;
	[SerializeField]
	private GameLoop.Settings gameLoop;
	[SerializeField]
	private Player.Settings player;

    public override void InstallBindings()
    {
        Container.BindInstance(serverConfiguration).AsSingle();
		Container.BindInstance(gameLoop).AsSingle();
		Container.BindInstance(player).AsSingle();
    }
}