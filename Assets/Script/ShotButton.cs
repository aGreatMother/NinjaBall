using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShotButton : MonoBehaviour {

	//Do this when the cursor enters the rect area of this selectable UI object.
	public static RectTransform rect;
	void Start(){
		rect = this.GetComponent<RectTransform> ();
	}
	public void GetInterative(){
		this.GetComponent<Button> ().interactable = true;
	}
	public void GetNonInterative(){
		this.GetComponent<Button> ().interactable = false;
	}
	public void SetOutBollFalse(){
		this.gameObject.GetComponent<Animator> ().SetBool ("out", false);
	}
}
