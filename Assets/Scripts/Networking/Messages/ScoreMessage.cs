using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ScoreMessage : BaseUdpMessage
{
	public int leftScore;
	public int rightScore;

	public ScoreMessage()
	{
	}

	public ScoreMessage(int leftScore, int rightScore)
	{
		this.leftScore = leftScore;
		this.rightScore = rightScore;
	}

	public override void Deserialize(IPEndPoint remote, DataReader reader)
	{
		base.Deserialize(remote, reader);

		leftScore = reader.GetInteger();
		rightScore = reader.GetInteger();
	}

	public override void Serialize(DataWriter writer)
	{
		base.Serialize(writer);

		writer.Write(leftScore);
		writer.Write(rightScore);
	}
}
