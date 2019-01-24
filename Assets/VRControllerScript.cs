using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerScript : MonoBehaviour {
	
	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int)trackedObj.index); } }

	// Use this for initialization
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	public bool GetGripInput()
	{
		return controller.GetPress (Valve.VR.EVRButtonId.k_EButton_Grip);
	}

	public float GetMagnitude()
	{
        return controller.velocity.magnitude;
	}

	public float GetsqrMagnitude()
	{
        return controller.velocity.sqrMagnitude;
	}

	public Vector3 GetVelocity()
	{
        return controller.velocity;
	}

	public void SetHapticFeedback(ushort Strength)
	{
		SteamVR_Controller.Input ((int)trackedObj.index).TriggerHapticPulse (Strength);
	}

	public SteamVR_Controller.Device GetController()
	{
		return controller;
	}
}
