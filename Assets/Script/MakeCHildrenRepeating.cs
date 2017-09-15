using UnityEngine;
using System.Collections;

public class MakeCHildrenRepeating : MonoBehaviour {
	private SpriteRenderer[] subs;

	// Use this for initialization
	private Vector3 moveDirect;
	private bool start=false;

	public Sprite[] sprites;
	public float randomMin = -2f;
	public float randomMax=2f;
	void Start(){

	}

	void OnRenderObject()  {
		subs = this.GetComponentsInChildren<SpriteRenderer> ();
		SortSubsByPosition ();
		moveDirect = GameObject.FindObjectOfType<FloorPart>().transform.right;
		start = true;
	}

	// Update is called once per frame
	void Update () {
		if (start) {


			if (!subs [0].isVisible&&subs[0].transform.position.x<Camera.main.transform.position.x) {
				//subs [0].transform.localEulerAngles = Vector3.forward * Random.Range (0, 360);
				if (Random.Range (0, 2) == 1)
					subs [0].transform.localScale = -1f * Mathf.Abs (subs [0].transform.localScale.x) * Vector3.right +
					subs [0].transform.localScale.y * Vector3.up;
				subs [0].transform.position = (Vector3)(Vector2)subs[subs.Length-1].transform.position+
					(subs[0].bounds.size.x/2+subs[0].transform.position.z*0.8f)* moveDirect
					+Vector3.forward*subs [0].transform.position.z+Vector3.right*Random.Range(randomMin,randomMax);


				subs [0].sprite = sprites [Random.Range (0, sprites.Length)];
				SortSubsByPosition ();
			}
		}


	}



	void SortSubsByPosition(){//from low to high
		int i,j;
		for (i = 0; i < subs.Length-1; i++)
			for (j = i; j < subs.Length; j++)
				if (subs [i].transform.position.x > subs [j].transform.position.x) {
					SpriteRenderer temp=subs[i];
					subs [i] = subs [j];
					subs [j] = temp;

				}



	}

}
