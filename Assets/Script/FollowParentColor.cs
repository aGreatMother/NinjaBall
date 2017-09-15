using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FollowParentColor : MonoBehaviour {
	private Image parentImage;
	// Use this for initialization
	void Start () {
		parentImage = this.GetComponentInParent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Text> ().color = parentImage.color;
	}
}
