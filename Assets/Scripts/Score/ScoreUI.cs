using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	[SerializeField]
	private Text _textBox;

	public void SetScore(int leftScore, int rightScore)
	{
		_textBox.text = $"{leftScore} : {rightScore}";
	}
}
