using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ControlManager : MonoBehaviour {
	[Header("UI")]
	public Button shotButton;
	public GameObject[] pcVshotNotes;// spicific note for PC version

	// Use this for initialization
	void Start () {
		#if !UNITY_ANDROID
		for(int i = 0;i<pcVshotNotes.Length;i++){
			pcVshotNotes[i].SetActive(true);}
		shotButton.interactable=false;
		#endif 
	}
	
	// Update is called once per frame
	void Update () {
		#if !UNITY_ANDROID
		if(Input.GetButtonDown("shot")||Input.GetMouseButtonDown(1))
			Singleton<MakeRope>.Instance.ShotOrCut();
		#endif
	}
}
