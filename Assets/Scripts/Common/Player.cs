using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
	[SerializeField]
	private CharacterController controller;

	public string UserName { get; set; }

	public void Move(Vector3 motion)
	{
		controller.Move(motion);
	}

    public class Factory : PlaceholderFactory<Player>
    {
    }
}
