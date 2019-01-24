using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.IO;

public class SwordProjectileScript : MonoBehaviour {

	static float waitTime = 0.0f;

	float SpinSpeed = 0.0f;
	bool Rotating = false;
	bool Ready = false;
    public VRControllerScript controllerScript;
	public GameObject Sword;
	public GameObject ChildSword;
    public GameObject SpellLights;
    public GameObject Spell;
    public GameObject FireSpellEffects;
    public Transform ForwardLocation;

	private Light[] Lights;
	private ParticleSystem[] Fire;

	public ParticleSystem SwordSpinningEffect;

	public bool Fireball = false;
	public bool SwordShooting = false;
	public float FireballMagnitude = 0.0f;

	bool SwordSpinSound = false;
	bool SwordSwingSound = false;
	Vector3 lastPosition;

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
		Lights = SpellLights.GetComponentsInChildren<Light> ();
		Fire = FireSpellEffects.GetComponentsInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 velocity = lastPosition - transform.position;
		lastPosition = transform.position;
		if (velocity.magnitude > 1.0f) 
			velocity.Normalize ();

		if (velocity.magnitude > 0.025f && !SwordSwingSound) {
			//AkSoundEngine.PostEvent ("Sword_Swing", this.gameObject);
			SwordSwingSound = true;
		}
		//AkSoundEngine.SetRTPCValue ("Sword_Velocity", Map(velocity.magnitude, 0.00f, 0.20f, 0.0f, 1.0f));

		if (SwordSwingSound ) {
			if (Wait(0.5f)) {
				SwordSwingSound = false;
			}
		}

		if (Fireball) 
			FireBall ();
		if (SwordShooting) 
        	SwordSummon ();
	}

	void FireBall()
	{

        if (controllerScript.GetController ().GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			for (int i = 0; i < 3; i++) {
				Lights [i].intensity = Random.Range (0.0f, Map(FireballMagnitude, 0.0f, 500.0f, 0.0f, 8.0f));
				Lights [i].range = Random.Range (0.0f, Map(FireballMagnitude, 0.0f, 500.0f, 0.0f, 0.55f));
                controllerScript.SetHapticFeedback((ushort)Random.Range(0, Map(FireballMagnitude, 0.0f, 3000.0f, 0.0f, 3999f)));
                FireballMagnitude += controllerScript.GetMagnitude () * 0.65f;
				FireballMagnitude = Mathf.Clamp (FireballMagnitude, 0.0F, 3000.0f);

				if(Fire[3].isPlaying == false)
					Fire[3].Play ();

				if (FireballMagnitude >= 400.0f) 
					if (Fire[0].isPlaying == false) 
						Fire [0].Play ();

				if (FireballMagnitude >= 2500.0f) 
					foreach (var fire in Fire) 
						if (fire.isPlaying == false) 
							fire.Play ();
			}
		} else {
			for (int i = 0; i < 3; i++) 
				Lights [i].intensity = 0.0f;
			
			foreach (var particle in Fire) 
				if (particle.isPlaying) 
					particle.Stop ();
		}

        if (controllerScript.GetController().GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			if (FireballMagnitude >= 400.0f) {
				GameObject spell = Instantiate(Spell, ForwardLocation.position, ForwardLocation.rotation) as GameObject;
				spell.GetComponent<Rigidbody>().AddForce(spell.transform.forward * Map(FireballMagnitude, 300.0f, 3000.0f, 0.0f, 250.0f),  ForceMode.VelocityChange);
				spell.transform.localScale = new Vector3(
					spell.transform.localScale.x + Map(FireballMagnitude, 0.0f, 3000.0f, -0.15f, 0.65f), 
					spell.transform.localScale.y + Map(FireballMagnitude, 0.0f, 3000.0f, -0.15f, 0.65f),
					spell.transform.localScale.z + Map(FireballMagnitude, 0.0f, 3000.0f, -0.15f, 0.65f)
				);
				InstructionsWallScript.UsedFireBall = true;
				Destroy (spell, 35.0f);
				//AkSoundEngine.PostEvent ("Fireball_Launch", this.gameObject);
			}

			FireballMagnitude = 0.0f;

			foreach (var particle in Fire) 
				if (particle.isPlaying) 
					particle.Stop ();
		}
	}
   
    void SwordSummon()
    {
		controllerScript.SetHapticFeedback((ushort)Map((short)SpinSpeed, 0, 25, 0, 3999));

        if (SpinSpeed == 25.0f) 
            Ready = true;
        else
            Ready = false;

		if (Ready && !SwordSpinningEffect.isPlaying) {
			SwordSpinningEffect.Play ();
		}

		if (SpinSpeed > 0.0f && !SwordSpinSound) {
			SwordSpinSound = true;
			//AkSoundEngine.PostEvent ("Play_Sword_Spin", this.gameObject);
		}

		//AkSoundEngine.SetRTPCValue ("Sword_Spin_Velocity", SpinSpeed);

        if (controllerScript.GetGripInput()) {
            SpinSpeed += controllerScript.GetMagnitude() * 0.20f;
            SpinSpeed = Mathf.Clamp (SpinSpeed, 0.0f, 25.0f);
            transform.Rotate (Vector3.forward, SpinSpeed);
            Rotating = true;
        }

        if (!controllerScript.GetGripInput() && Rotating) {
            SpinSpeed -= Time.deltaTime * 40;
            SpinSpeed = Mathf.Clamp (SpinSpeed, 0.0f, 25.0f);
            transform.Rotate (Vector3.forward, SpinSpeed);
            Rotating = true;
			InstructionsWallScript.UsedSwordShot = true;
        }

        if (SpinSpeed == 0.0f) 
            Rotating = false;

        if (!controllerScript.GetGripInput() && Ready) {
            GameObject sword = Instantiate(Sword, ForwardLocation.position, ForwardLocation.rotation) as GameObject;
            sword.GetComponentInChildren<Rigidbody>().AddForce(sword.transform.forward * 50.0f,  ForceMode.VelocityChange);
			sword.tag = "SwordProjectile";
            Destroy (sword, 5.0f);
            Ready = false;
			SwordSpinningEffect.Stop ();
			//AkSoundEngine.PostEvent ("Sword_Launch_SFX", this.gameObject);
        }

		if (!controllerScript.GetGripInput() && SwordSpinSound) {
			//AkSoundEngine.PostEvent ("Stop_Sword_Spin", this.gameObject);
			SwordSpinSound = false;
		}
    }

	float Map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

	short Map(short x, short in_min, short in_max, short out_min, short out_max)
	{
		return (short)((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
	}

	bool Wait(float time)
	{
		waitTime += Time.deltaTime;
		if (waitTime >= time) {
			waitTime = 0.0f;
			return true;
		}
		return false;
	}
}
