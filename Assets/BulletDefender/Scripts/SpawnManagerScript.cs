using UnityEngine;
using System.Collections;

public class SpawnManagerScript : MonoBehaviour {

	public float SpawnCooldown = 1f;
	public Transform[] SpawnLocations;
	public Transform Target;
	public GameObject[] Enemies;

	private float Timer = 0f;

	// Use this for initialization
	void Start () {
		Vector3 playerToMouse = Target.position;
		playerToMouse.z = 0f;
		Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

		Instantiate(Enemies[0], SpawnLocations[0].position, newRotation);
	}
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (Timer > SpawnCooldown) {
			Vector3 playerToMouse = Target.position;
			playerToMouse.z = 0f;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			Instantiate(Enemies[Random.Range(0 , Enemies.Length)], SpawnLocations[Random.Range(0, SpawnLocations.Length)].position, newRotation);
			Timer = 0;
		}
	}
}
