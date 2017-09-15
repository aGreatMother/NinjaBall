using UnityEngine;
using System.Collections;

public class Snail : MonoBehaviour {

	private GameObject snailShodow;
	// Use this for initialization
	void Start () {
		snailShodow = transform.Find ("snailShadow").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D (Collider2D coll){
		if (coll.gameObject.name == "ball") {
			snailShodow.GetComponent<Animator> ().Play ("snailEat");
			snailShodow.transform.right = this.transform.position - snailShodow.transform.position;
			this.GetComponent<Animator> ().Play ("snailDisap");
			this.GetComponent<Collider2D> ().enabled = false;
			this.GetComponent<AudioSource> ().Play ();
			Singleton<GameManagerCM>.Instance.GetSnail ();
		}
	}
}
