using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour {
	public int Damage = 20;
	public SwordProjectileScript SwordProjectileScriptObject;
    public VRControllerScript controllerScript;
	Vector3 SavedPosition = Vector3.zero;

	public bool SavePosition = false;

	void Start()
	{
		SavedPosition = transform.localPosition;

		if (controllerScript == null) {
			Debug.Log ("Controller was null at Start");
			controllerScript = GetComponentInParent<VRControllerScript> ();
		}
	}

	void LateUpdate()
	{
		if(SavePosition)
			transform.localPosition = SavedPosition;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Fireball Pickup")) {
			SwordProjectileScriptObject.Fireball = true;
			collision.gameObject.transform.parent = this.transform;
			collision.gameObject.transform.localPosition = this.transform.localPosition;
			Destroy (collision.gameObject, 1.5f);
			return;
		}
	}

    public VRControllerScript getVRControllerScript()
    {
		if (controllerScript == null) {
			controllerScript = GetComponentInParent<VRControllerScript> ();
			Debug.Log ("Controller was null at Function Call");
		}
        return controllerScript;
    }
}