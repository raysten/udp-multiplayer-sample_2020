using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMessageHandler : BaseHandler<ScoreMessage>
{
	private ScoreUI _scoreUI;

	public ScoreMessageHandler(MessageProcessor messageProcessor, ScoreUI scoreUI) : base(messageProcessor)
	{
		_scoreUI = scoreUI;
	}

	public override void Handle(ScoreMessage message)
	{
		_scoreUI.SetScore(message.leftScore, message.rightScore);
	}
}
