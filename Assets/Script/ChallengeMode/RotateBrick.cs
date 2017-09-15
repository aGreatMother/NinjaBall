using UnityEngine;
using System.Collections;
using DG.Tweening;
public class RotateBrick : MonoBehaviour {

	public float gap=2f;
	public float duration = 1f;
	public float angle = 60f;

	private float gapCount = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gapCount < gap) {
			gapCount += Time.deltaTime;
		
		} else {
			gapCount = 0f;
			this.transform.DORotate (transform.eulerAngles + angle * transform.forward, duration);
		
		}
	
	}
}
