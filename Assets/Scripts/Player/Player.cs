using System;
using UnityEngine;
using Zenject;

public abstract class Player : MonoBehaviour, IUpdatable
{
	[Inject]
	protected float _speed;
	[Inject]
	protected GameLoop _loop;

	public int PlayerId { get; set; }

	public void Start()
	{
		_loop.Subscribe(this);
	}

	public abstract void Simulate(uint tickIndex);

	[Serializable]
	public class Settings
	{
		public float speed = 2f;
	}
}
