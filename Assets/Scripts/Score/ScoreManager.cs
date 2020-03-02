using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
	private Server _server;
	private ScoreUI _scoreUI;

	public ScoreManager(Server server, ScoreUI scoreUI)
	{
		_server = server;
		_scoreUI = scoreUI;
	}

	public int LeftScore { get; set; }
	public int RightScore { get; set; }

	public void PropagateScore()
	{
		_scoreUI.SetScore(LeftScore, RightScore);
		var scoreMessage = new ScoreMessage(LeftScore, RightScore);
		_server.SendToAll(scoreMessage);
	}
}
