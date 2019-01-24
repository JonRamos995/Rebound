using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScriptVR : MonoBehaviour {

	SteamVR_PlayArea playArea;

	public GameObject Enemy;
	public float delay = 2.0f;
	static public float Timer = 0.0f;
	public float timerDelay = 0.0f;
	public float MagnitudeAdjust = 0.1f;
	static public float avgMagnitude = 0f;
	static public bool START = false;

    float newDelay = 1.5f;

	// Use this for initialization
	void Start () {
		playArea = GetComponent<SteamVR_PlayArea> ();

		foreach (var vertex in playArea.vertices) 
			avgMagnitude += vertex.magnitude;

		avgMagnitude /= playArea.vertices.Length;
		avgMagnitude -= MagnitudeAdjust;
	}
	
	// Update is called once per frame
	void Update () {
		if (START) {
			Timer += Time.deltaTime;
			timerDelay += Time.deltaTime;

			if (timerDelay > delay) {
				timerDelay = 0.0f;

				Vector3 position = Random.insideUnitSphere * avgMagnitude;

				if (position.y < 0.0f) {
					position = new Vector3 (
						position.x, 
						position.y * -1,
						position.z
					);

					if (position.y < 0.5f) {
						position = new Vector3 (
							position.x, 
							position.y + 1.0f,
							position.z
						);
					}
				}

				GameObject pointer = Instantiate (Enemy, position, Quaternion.identity) as GameObject;
				pointer.transform.localScale = transform.localScale / 4;
				pointer.name = "Enemy";
				Destroy (pointer, 30.0f);

                if (Timer > 180)
                    delay = newDelay;
                else
                    delay = 2.0f;
                   

				if (Timer > 240.0f) 
					START = false;
			}
		}
	}
}
