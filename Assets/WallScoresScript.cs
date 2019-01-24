using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScoresScript : MonoBehaviour {

	public static uint Hits = 0;
	uint lastHits = 0;
	public static float Timer = 0.0f;
	float Minutes = 0.0f;
	float Seconds = 0.0f;

	TextMesh[] Texts;
	// Use this for initialization
	void Start () {
		Texts = GetComponentsInChildren<TextMesh> ();
		Texts [1].text = "Hits : " + Hits.ToString ();
	}

	// Update is called once per frame
	void Update () {
		if (Hits != lastHits) {
			// Update Text
			lastHits = Hits;
			Texts [1].text = "Hits : " + Hits.ToString ();
		}
		if (EnemySpawnerScriptVR.START) 
			Timer += Time.deltaTime;
	}

	void LateUpdate()
	{
		Minutes = Mathf.Floor (Timer / 60);
		Seconds = Mathf.Round (Timer % 60);

		Texts [0].text = "Time  \n" + ((Minutes > 9.0f) ? Minutes.ToString() : "0" + Minutes.ToString()) + " : " + ((Seconds > 9.0f) ? Seconds.ToString() : "0" + Seconds.ToString());
	}
}
