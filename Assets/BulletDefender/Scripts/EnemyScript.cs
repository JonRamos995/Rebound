using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public int Lives = 5;
	public Transform Target;
	public float Speed;

	void FixedUpdate()
	{
		float speed = Speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, Target.position, speed);
		//transform.LookAt (Target.position);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag ("PlayerBullet")) {
			if (Lives < 1) {
				Destroy (this.gameObject);
			} 
		} else
			return;
	}

	public int GetLives()
	{
		return Lives;
	}

	public void DecreaseLives(int _value)
	{
		Lives -= _value;
	}
}
