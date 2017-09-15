using UnityEngine;
using System.Collections;

public class DestorWhenGone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ballLocation = GameObject.Find ("ball").transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( hadBeenSeen&&ballLocation.position.x>this.transform.position.x+10f)
			Destroy (this.gameObject);
	}
	private bool hadBeenSeen=false;
	private Transform ballLocation;
	void OnBecameVisible(){
		hadBeenSeen = true;
	}


}
