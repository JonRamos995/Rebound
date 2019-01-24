using UnityEngine;
using System.Collections;

public class EnemySwordScript : MonoBehaviour {

	public float DeathTimer = 5.0f;
	public int Lives = 5;
	public float Speed;
	public float SafeDistance = 0.5f;

	float MOVEMultiplier = 1.0f;

	public bool MOVE = false;
	public bool DESTROY = false;
	public bool FIREONLY = false;

	bool Close = false;

	public float multiplier = 4.0f;

	GameObject HMD;

	Vector3 acceleration = Vector3.one;

    Vector3 movement;

	void Start()
	{
        movement = Random.insideUnitSphere.normalized;

		if (Random.Range(0.0f, 100.0f) < 30.0f) 
			MOVE = true;
		
		if(DESTROY)
			Destroy (gameObject, DeathTimer);

		HMD = GameObject.FindGameObjectWithTag ("MainCamera");

		if (HMD == null) 
			Debug.Log ("HMD NOT initialized in EnemySwordScript");

		multiplier = (multiplier * (EnemySpawnerScriptVR.Timer / 60)) + 1.0f;
		multiplier = Mathf.Clamp (multiplier, 1.0f, 4.0f);

		if (EnemySpawnerScriptVR.Timer > 180) {
			Lives = 3;
			multiplier *= 1.5f;
		}

		assignColor ();

		if (MOVE) {
			SphereCollider[] colliders = GetComponents<SphereCollider> ();
			for (int i = 0; i < colliders.Length; i++) 
				if (colliders [i].isTrigger)
					colliders [i].radius = colliders [i].radius * 8;

			MOVEMultiplier = 5.0f;
			transform.localScale *= 2.0f;
			GetComponent<Rigidbody> ().drag *= 2.0f;
		}


	}

	void LateUpdate()
	{
		movement += Random.insideUnitSphere.normalized;

		if (movement.magnitude > 1.0f) 
			movement.Normalize();
		
		GetComponent<Rigidbody> ().AddForce ((movement * 2), ForceMode.Impulse);
		
		if (Close) {
			GetComponent<Rigidbody>().AddForce(acceleration * multiplier, ForceMode.Impulse);
		}

		// If the Enemy is too far away from the PlayArea, adds force to return it to the PlayArea
		if (transform.position.magnitude > (EnemySpawnerScriptVR.avgMagnitude * MOVEMultiplier) + 0.5f) {
			GetComponent<Rigidbody> ().AddForce (HMD.transform.position - transform.position, ForceMode.Impulse);
		}
	}

	void OnCollisionEnter (Collision other)
	{
		
		if (other.collider.CompareTag ("PlayerSword") || other.collider.CompareTag("FireSpell")) {

			VRControllerScript controller = other.gameObject.GetComponent<SwordScript> ().getVRControllerScript ();

			if (controller != null) {
				// Controller Feedback
				for (int i = 0; i < 10; i++)
					controller.SetHapticFeedback (3999);
			}

			//if (other.collider.CompareTag("PlayerSword"))
			//	AkSoundEngine.PostEvent ("Sword_Orb_Hit", this.gameObject);

			// Increases the Hits count in the wall
			WallScoresScript.Hits++;

			// Lower the Enemies health
			Lives -= 1;

			// Destroyes the Enemy if its Life reaches 0 or it is hit with a FireBall
			if (Lives <= 0 || other.gameObject.CompareTag ("FireSpell")) {
				Destroy (gameObject);
				return;
			}

			// Adds Impulse to the object
			if(controller != null)
				GetComponent<Rigidbody> ().AddForce (controller.GetVelocity().normalized * 30, ForceMode.Impulse);
			else
				GetComponent<Rigidbody> ().AddForce (other.gameObject.GetComponent<Rigidbody>().velocity.normalized * 30, ForceMode.Impulse);

			// Changes color accordingly
			assignColor ();

		} else {

			if (other.collider.CompareTag("SwordProjectile")) {
				// Lower the Enemies health
				Lives -= 1;

				// Destroyes the Enemy if its Life reaches 0 or it is hit with a FireBall
				if (Lives <= 0 || other.gameObject.CompareTag ("FireSpell")) {
					Destroy (gameObject);
					return;
				}

				// Changes color accordingly
				assignColor ();

				GetComponent<Rigidbody> ().AddForce (other.gameObject.GetComponent<Rigidbody>().velocity.normalized * 30, ForceMode.Impulse);
			}

			// Adds force to the Enemy if it is hit with something that is NOT the player
			GetComponent<Rigidbody> ().AddForce (other.impulse, ForceMode.Impulse);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PlayerSword")) {
			Close = true;

			VRControllerScript controller = other.gameObject.GetComponent<SwordScript> ().getVRControllerScript ();

			acceleration = transform.position - other.transform.position;
			acceleration.Normalize ();
			if (controller == null) 
				acceleration = Vector3.Cross (acceleration, GetComponent<Rigidbody>().velocity.normalized);
			else
				acceleration = Vector3.Cross (acceleration, controller.GetVelocity().normalized);
			return;
		}

		if (other.CompareTag("SwordProjectile")) {
			acceleration = transform.position - other.transform.position;
			acceleration.Normalize ();
			Debug.LogWarning ("SwordProjectile Match!");
			acceleration = Vector3.Cross (acceleration, other.gameObject.GetComponent<Rigidbody>().velocity.normalized);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag ("PlayerSword")) {
			acceleration = transform.position - other.transform.position;
			float distance = acceleration.magnitude;

			if (distance < SafeDistance) {
				acceleration.Normalize ();
				acceleration *= (SafeDistance - distance) / SafeDistance;
			}

			if (acceleration.magnitude > 1.0f) 
				acceleration.Normalize ();
			
			VRControllerScript controller = other.gameObject.GetComponent<SwordScript> ().getVRControllerScript ();

			if (controller == null) 
				acceleration = Vector3.Cross (acceleration, GetComponent<Rigidbody>().velocity.normalized);
			else
				acceleration = Vector3.Cross (acceleration, controller.GetVelocity().normalized);
			return;
		}

		if (other.CompareTag("SwordProjectile")) {
			acceleration = transform.position - other.transform.position;
			acceleration.Normalize ();
			acceleration = Vector3.Cross (acceleration, other.gameObject.GetComponent<Rigidbody>().velocity.normalized);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("PlayerSword")) {
			Close = false;
		}
	}

	void assignColor()
	{
		if (MOVE) {
			if (Time.realtimeSinceStartup > 180 && Lives == 3) 
				GetComponent<MeshRenderer> ().material.color = Color.yellow;

			if (Lives == 2) 
				GetComponent<MeshRenderer> ().material.color = Color.magenta;

			if (Lives == 1) 
				GetComponent<MeshRenderer> ().material.color = Color.cyan;
			return;
		}


		if (Time.realtimeSinceStartup > 180 && Lives == 3) 
			GetComponent<MeshRenderer> ().material.color = Color.green;
		
		if (Lives == 2) 
			GetComponent<MeshRenderer> ().material.color = Color.blue;
		
		if (Lives == 1) 
			GetComponent<MeshRenderer> ().material.color = Color.red;
	}
}
