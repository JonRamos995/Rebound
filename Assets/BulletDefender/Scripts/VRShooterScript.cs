using UnityEngine;
using System.Collections;

public class VRShooterScript : MonoBehaviour {
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int)trackedObj.index); } }
	public GameObject Bullet;
	public GameObject SpawnLocation;
	public float BulletCooldown = 0.30f;

	public Color color;
	public float thickness = 0.002f;
	public GameObject holder;
	public GameObject pointer;
	public bool addRigidBody = false;

	private float Timer = 0;

	private AudioSource[] audioSources;
	private AudioClip audioClip;
	private int AudioCounter = 0;


	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		InstantiateVRLazer ();
		audioSources = SpawnLocation.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward);
		Timer += Time.deltaTime;
		if (Timer >= BulletCooldown && controller != null && controller.GetPress(triggerButton)) {
			Fire ();
			Timer = 0;
		}
	}

	void Fire()
	{
		Ray raycast = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(raycast, out hit)) {
			
			Vector3 ControllerAim = hit.point - SpawnLocation.transform.position;
			Quaternion newRotation = Quaternion.LookRotation (ControllerAim);
			PlaySound();
			Instantiate (Bullet, SpawnLocation.transform.position, newRotation);
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

	void InstantiateVRLazer()
	{
		holder = new GameObject();
		holder.transform.parent = this.transform;
		holder.transform.localPosition = Vector3.zero;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
		pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		BoxCollider collider = pointer.GetComponent<BoxCollider>();
		if (addRigidBody)
		{
			if (collider)
			{
				collider.isTrigger = true;
			}
			Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
			rigidBody.isKinematic = true;
		}
		else
		{
			if(collider)
			{
				Object.Destroy(collider);
			}
		}
		Material newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);
		pointer.GetComponent<MeshRenderer>().material = newMaterial;
	}

}
