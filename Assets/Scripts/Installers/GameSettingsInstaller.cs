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
	[SerializeField]
	private BallSettings ballSettings;
	[SerializeField]
	private RemoteEntity.Settings remoteEntity;

    public override void InstallBindings()
    {
        Container.BindInstance(serverConfiguration).AsSingle();
		Container.BindInstance(player).AsSingle();
		Container.BindInstance(clockAdjustmentCurve).AsSingle();
		Container.BindInstance(reconcilation).AsSingle();
		InstallBall();
		Container.BindInstance(remoteEntity).AsSingle();
    }

	private void InstallBall()
	{
		Container.BindInstance(ballSettings.ball).AsSingle();
		Container.BindInstance(ballSettings.kick).AsSingle();
	}

	[Serializable]
	public class BallSettings
	{
		public Ball.Settings ball;
		public BallKick.Settings kick;
	}
}