using System;
using UnityEngine;
using Zenject;

public abstract class Player : MonoBehaviour, IUpdatable
{
	private Renderer _rend;
	private MaterialPropertyBlock _propertyBlock;

	[Inject]
	protected Team _team;
	[Inject]
	protected GameLoop _loop;
	[Inject]
	protected Settings _settings;

	public int PlayerId { get; set; }
	public string ClientId { get; set; }
	public Team Team => _team;

	public void Start()
	{
		_loop.Subscribe(this);
		_rend = GetComponent<Renderer>();
		_propertyBlock = new MaterialPropertyBlock();
		_rend.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetColor("_Color", _team == Team.Left ? _settings.leftTeamColor : _settings.rightTeamColor);
		_rend.SetPropertyBlock(_propertyBlock);
	}

	public abstract void Simulate(uint tickIndex);

	[Serializable]
	public class Settings
	{
		public float speed = 2f;
		public Color leftTeamColor;
		public Color rightTeamColor;
	}
}

public enum Team { Left, Right }
