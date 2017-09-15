using UnityEngine;
using System.Collections;

public class BallsFace : MonoBehaviour {
	private Renderer ballsRenderer=null;
	public GameObject Rstreamer;
	public GameObject Lstreamer;
	public CircleCollider2D coll;
	private float orginCollSize;

	private float streamerRotation;
	private float streamerRotation1;
	private float streamerStartSpeed;
	private bool On = false;
	// Use this for initialization
	void Start () {
		ballsRenderer = this.transform.parent.GetComponent<Renderer> ();
		orginCollSize = coll.radius;
		StopStreamer (Lstreamer);
		StopStreamer (Rstreamer);
	}

	public void HideOrShowBallBody(){
		if (ballsRenderer.enabled) {
			On = true;
			coll.radius = orginCollSize + 0.15f;
			ballsRenderer.enabled = false;
		} else {
			On = false;
			coll.radius = orginCollSize;
			ballsRenderer.enabled = true;
		}
	}
	void StopStreamer(GameObject streamer){
		ParticleSystem[] ps = streamer.GetComponentsInChildren<ParticleSystem> ();
		for (int i = 0; i < ps.Length; i++) {
			ps [i].Stop();
		}
	}

	void StartStreamer(GameObject streamer){
		ParticleSystem[] ps = streamer.GetComponentsInChildren<ParticleSystem> ();
		for (int i = 0; i < ps.Length; i++) {
			ps [i].Play();
		}
	}

	float lastScaleX;
	// Update is called once per frame
	void Update () {
		if (!On) {
			StopStreamer (Lstreamer);
			StopStreamer (Rstreamer);
		}

		if (On&&lastScaleX != transform.localScale.x) {
			if (transform.localScale.x > 0) {
				
				StartStreamer (Rstreamer);
				StopStreamer (Lstreamer);
			} else {
				StartStreamer (Lstreamer);
				StopStreamer (Rstreamer);
			}

			lastScaleX = transform.localScale.x;
		}

	}
	public void Dead(){
		ballsRenderer.enabled = false;
		StopStreamer (Lstreamer);
		StopStreamer (Rstreamer);
		this.enabled = false;
	}

	public void AddStreamer(){
		if (transform.localScale.x > 0) {
			StartStreamer (Rstreamer);
			StopStreamer (Lstreamer);
		

	} else {
		StartStreamer (Lstreamer);
		StopStreamer (Rstreamer);
	}

}

}
