using UnityEngine;
using System.Collections;

public class SwordSpawnManagerScript : MonoBehaviour {
	public float StartDelay = 3.0f;
	public float SpawnCooldown = 1f;
	public GameObject[] Enemies;

	private float Timer = 0f;
	private Transform[] SpawnLocations;
	private GameObject HMD;
	bool StartSpawn = false;

	// Use this for initialization
	void Start () {
		HMD = GameObject.FindGameObjectWithTag ("MainCamera");  // Gets the GameObject of the VIVE Headset
		SpawnLocations = GameObject.FindGameObjectWithTag ("SpawnPoints").GetComponentsInChildren<Transform> (); // Transform Locations where the Sphere enemies Spawn from
	}

	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (!StartSpawn && Timer > StartDelay) 
			StartSpawn = true;

		if (Timer > SpawnCooldown && StartSpawn) {
			Vector3 playerToMouse = HMD.transform.position;
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			Instantiate(Enemies[Random.Range(0 , Enemies.Length)], SpawnLocations[Random.Range(0, SpawnLocations.Length)].position, newRotation);  // Spawns an Enemy
			Timer = 0;
		}
	}
}