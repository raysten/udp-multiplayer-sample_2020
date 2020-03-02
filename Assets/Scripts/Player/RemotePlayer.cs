using UnityEngine;
using Zenject;

public class RemotePlayer : Player, IRemoteEntity
{
	private RemoteEntity _entity;

	[Inject]
	public void Construct(RemoteEntity.Settings entitySettings)
	{
		_entity = new RemoteEntity(transform, entitySettings);
	}

	public override void Simulate(uint tickIndex)
	{
		_entity.Simulate();
	}

	public void EnqueuePosition(Vector3 position, uint tickIndex)
	{
		_entity.EnqueuePosition(position, tickIndex);
	}

	public class Factory : PlaceholderFactory<Team, RemotePlayer>
	{
	}
}
