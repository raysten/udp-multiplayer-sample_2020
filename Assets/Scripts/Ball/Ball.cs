using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour, IUpdatable, IRemoteEntity
{
	[SerializeField]
	private bool _isClient;
	[SerializeField]
	private float _lerpSpeed = 3f;

	private GameLoop _loop;
	private ScoreManager _score;
	private Settings _settings;
	private RemoteEntity _entity;

	public Rigidbody Rigidbody { get; private set; }

	[Inject]
	public void Construct(
		GameLoop loop,
		[InjectOptional] ScoreManager score,
		Settings settings,
		RemoteEntity.Settings entitySettings
	)
	{
		_loop = loop;
		_score = score;
		_settings = settings;
		_entity = new RemoteEntity(transform, entitySettings);
	}

	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();

		if (_isClient)
		{
			_loop.LateSubscribe(this);
		}
	}

	public void Simulate(uint tickIndex)
	{
		_entity.Simulate();
	}

	public void EnqueuePosition(Vector3 position, uint tickIndex)
	{
		_entity.EnqueuePosition(position, tickIndex);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (_isClient == false && (other.tag == _settings.leftGoalTag || other.tag == _settings.rightGoalTag))
		{
			transform.position = new Vector3(0f, 3f, 0f);
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;

			if (_score != null)
			{
				if (other.tag == _settings.leftGoalTag)
				{
					_score.RightScore++;
				}
				else if (other.tag == _settings.rightGoalTag)
				{
					_score.LeftScore++;
				}

				_score.PropagateScore();
			}
		}
	}

	[System.Serializable]
	public class Settings
	{
		public string leftGoalTag;
		public string rightGoalTag;
	}
}
