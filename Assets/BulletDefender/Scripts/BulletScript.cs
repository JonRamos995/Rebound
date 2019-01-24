using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public float force;
	public int Damage = 2;

	void Start ()
	{
		GetComponent<Rigidbody> ().AddForce (transform.forward * (force * 100));
		Destroy (this.gameObject, 5.0f);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Enemy")) {
			other.collider.GetComponent<EnemyScript> ().DecreaseLives (Damage);
			Destroy (this.gameObject);
		}
	}

}


