using UnityEngine;
using System.Collections;

public class reward : MonoBehaviour {

	private Animator anim;
	private bool eaten=false;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();

	}
	
	// Update is called once per frame

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.gameObject.name == "ball") {
			GameObject.FindObjectOfType<Ball> ().StartFireOn ();
			anim.Play ("rewardEat");
			eaten = true;
			this.GetComponent<Collider2D> ().enabled = false;
		}
	}



}
