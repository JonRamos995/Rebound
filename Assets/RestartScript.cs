using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RestartScript : MonoBehaviour {

	void OnCollisionEnter(Collision other)
	{
		WallScoresScript.Hits = 0;
        WallScoresScript.Timer = 0.0f;
		EnemySpawnerScriptVR.START = true;
		EnemySpawnerScriptVR.Timer = 0.0f;
	}
}
