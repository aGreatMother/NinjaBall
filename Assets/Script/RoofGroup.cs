
using UnityEngine;
using System.Collections;

public class RoofGroup : MonoBehaviour {

	public Renderer[] subs;

	// Use this for initialization
	private Vector3 moveDirect;
	private bool start=false;
	private Transform ball;
	void Start(){
		ball = GameObject.Find ("ball").transform;
	}

	void OnRenderObject()  {
		SortSubsByPosition ();
		moveDirect = subs [1].transform.position - subs [0].transform.position;
		start = true;
	}

	// Update is called once per frame
	void Update () {
		if (start) {
			if (!subs [0].isVisible&&ball.transform.position.x>subs[1].transform.position.x
				&&subs[0].gameObject.transform.position!=subs [1].gameObject.transform.position+moveDirect) {
				subs [0].gameObject.transform.position = subs [1].gameObject.transform.position + moveDirect;
				Renderer temp = subs [0];
				subs [0] = subs [1];
				subs [1] = temp;
		
			}
			if (!subs [1].isVisible&&ball.transform.position.x<subs[0].transform.position.x
				&&subs[1].gameObject.transform.position!=subs [0].gameObject.transform.position-moveDirect) {
				subs [1].gameObject.transform.position = subs [0].gameObject.transform.position - moveDirect;
				Renderer temp = subs [0];
				subs [0] = subs [1];
				subs [1] = temp;

			}
		
		
		
		}
	}


	void SortSubsByPosition(){//from low to high

		if(subs[0].gameObject.transform.position.x>subs[1].gameObject.transform.position.x){
			Renderer temp = subs [0];
			subs [0] = subs [1];
			subs [1] = temp;



		}


	}
}
