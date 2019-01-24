using UnityEngine;
using System.Collections;

public class SwordBulletScript : MonoBehaviour {

	public float FORCE = 400f;

	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody> ().AddForce (transform.forward * (FORCE * 100));

        Destroy(gameObject, 30.0f);
	
	}

	void OnCollisionEnter(Collision other)
	{
        if (other.collider.CompareTag("Enemy"))
        {
		    Destroy (gameObject, 0.5f);
        }
        return;
	}
}
