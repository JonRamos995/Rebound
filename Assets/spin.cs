using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour {

	public float Speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.Rotate(transform.rotation * transform.up * (Time.deltaTime * Speed));
	}
}
