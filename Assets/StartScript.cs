using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		EnemySpawnerScriptVR.START = true;
		Destroy (this.gameObject, 1.0f);
	}
}
