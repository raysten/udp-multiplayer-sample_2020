using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "ServerSettings", menuName = "Installers/ServerSettings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    [SerializeField]
    private Server.Settings serverConfiguration;
	[SerializeField]
	private Player.Settings player;
	[SerializeField]
	private ServerClockMessageHandler.Settings clockAdjustmentCurve;
	[SerializeField]
	private PlayerReconciler.Settings reconcilation;

    public override void InstallBindings()
    {
        Container.BindInstance(serverConfiguration).AsSingle();
		Container.BindInstance(player).AsSingle();
		Container.BindInstance(clockAdjustmentCurve).AsSingle();
		Container.BindInstance(reconcilation).AsSingle();
    }
}