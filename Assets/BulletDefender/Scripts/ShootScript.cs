using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour {


	public GameObject Bullet;
	public AudioClip audioClip;
	public float BulletCooldown = 0.30f;

	int floorMask;
	float camRayLength = 100f;
	float Timer = 0;

	private AudioSource[] audioSources;
	private int AudioCounter = 0;

	void Awake()
	{
		audioSources = GetComponents<AudioSource> ();
		CreateAudioSources (audioSources.Length);
	}

	void Start () 
	{
		floorMask = LayerMask.GetMask ("Floor");
	}

	void Update()
	{
		Timer += Time.deltaTime;
		if (Timer >= BulletCooldown && Input.GetButton("Fire1")) {
			Fire ();
			Timer = 0;
		}
	}

	void Fire()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;

			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
		//	AudioSource.PlayClipAtPoint (audio.clip, transform.position);
			PlaySound();
			Instantiate (Bullet, transform.position, newRotation);
		}
	}


	void CreateAudioSources(int amount)
	{
		for (int i = 0; i < amount; i++) {
			audioSources[i].clip = audioClip;
			audioSources [i].playOnAwake = false;
			audioSources[i].volume = 0.8f;
		}
	}

	void PlaySound()
	{
		if (AudioCounter > audioSources.Length - 1) {
			AudioCounter = 0;
		}

		audioSources [AudioCounter].Play();
		AudioCounter++;
	}
}

