using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour, IUpdatable
{
	[SerializeField]
	private bool _isClient;
	[SerializeField]
	private float _lerpSpeed = 3f;

	private GameLoop _loop;

	public Vector3 TargetPosition { get; set; }

	[Inject]
	public void Construct(GameLoop loop)
	{
		_loop = loop;
	}

	private void Start()
	{
		if (_isClient)
		{
			TargetPosition = transform.position;
			_loop.LateSubscribe(this);
		}
	}

	public void Simulate(uint tickIndex)
	{
		//Vector3 direction = (TargetPosition - transform.position).normalized;
		//transform.position += direction * Time.fixedDeltaTime * _lerpSpeed;
		transform.position = TargetPosition;
	}

	private void OnTriggerEnter(Collider other)
	{
		// TODO: Detect goals.
	}
}
