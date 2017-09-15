using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {

	private SpriteRenderer render;
	public Sprite hookCloseRender;
	public Sprite hookOpenRender;
	public Transform followingPoint;
	// Use this for initialization
	void Awake () {
		render = this.GetComponent<SpriteRenderer> ();
	}

	public void Open(){
		render.sprite = hookOpenRender;
	}
	public void Close(){
		render.sprite = hookCloseRender;
	}
	// Update is called once per frame
	void Update () {
		if (followingPoint != null)
			this.transform.position = followingPoint.position;
	}
}
