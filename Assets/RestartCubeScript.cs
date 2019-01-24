using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartCubeScript : MonoBehaviour {
	Vector3 backup;
	float ratio = 0.0f;
	void Start()
	{
		backup = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (EnemySpawnerScriptVR.Timer > 240) {
			ratio += Time.deltaTime / 5.0f;
			ratio = Mathf.Clamp (ratio, 0.0f, 1.0f);
			transform.position = Vector3.Lerp(backup, new Vector3(0.0f, 0.8f,-1.35f), ratio);
		}
	}
}
