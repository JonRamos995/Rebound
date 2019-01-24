using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsWallScript : MonoBehaviour {

	MeshRenderer[] renderers;
	public GameObject Explosion;
	static public bool UsedFireBall = false;
	static public bool UsedSwordShot = false;

	void LateUpdate () {
		if (UsedFireBall && UsedSwordShot) {
			foreach (var explosion in Explosion.GetComponentsInChildren<ParticleSystem>()) 
				explosion.Play ();
			
			Destroy (this.gameObject, 1.0f);
		}
	}
}
