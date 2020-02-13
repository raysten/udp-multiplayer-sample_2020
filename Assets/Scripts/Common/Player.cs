using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IUpdatable
{
	[SerializeField]
	private CharacterController controller;

	[Inject]
	private float _speed;
	[Inject]
	private GameLoop _loop;

	//private int tickCounter;

	public string UserName { get; set; }
	public Vector3 Input { get; set; }

	public void Start()
	{
		_loop.Subscribe(this);
	}

	public void Simulate(uint tickIndex)
	{
		if (Input != Vector3.zero)
		{
			Debug.Log($"Moving player on tick {tickIndex}");
		}

		Move(Input);
		Input = Vector3.zero;
		//tickCounter++;
	}

	private void Move(Vector3 motion)
	{
		controller.Move(motion.normalized * Time.deltaTime * _speed);
	}

	public class Factory : PlaceholderFactory<float, Player>
    {
    }

	[Serializable]
	public class Settings
	{
		public float speed = 2f;
	}
}
