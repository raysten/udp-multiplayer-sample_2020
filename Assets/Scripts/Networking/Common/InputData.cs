using UnityEngine;

public class InputData
{
	public Vector3 input;
	public uint tickIndex;

	public InputData(Vector3 input, uint tickIndex)
	{
		this.input = input;
		this.tickIndex = tickIndex;
	}
}
