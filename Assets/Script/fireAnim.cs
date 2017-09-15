using UnityEngine;
using System.Collections;

public class fireAnim : MonoBehaviour {
	private ParticleSystem fireEffect;
	// Use this for initialization
	void Start () {
		fireEffect = this.GetComponentInChildren<ParticleSystem> ();
	}
	private void FireEffectOn(){
		fireEffect.Play ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
