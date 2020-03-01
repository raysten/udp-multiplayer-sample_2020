using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRemoteEntity
{
	void EnqueuePosition(Vector3 position, uint tickIndex);
}
