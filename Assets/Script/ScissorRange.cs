using UnityEngine;
using System.Collections;

public class ScissorRange : MonoBehaviour {

	private Scissors parentSci;
	// Use this for initialization
	void Start () {
		parentSci = this.GetComponentInParent<Scissors> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay2D(Collider2D col){
		if (col.tag == "rope2D" && Rope2D.exsitOne) {//when the rope is formed
			parentSci.GetTarget (col.gameObject.transform);
			this.GetComponent<Collider2D> ().enabled = false;
		}
	}

}
