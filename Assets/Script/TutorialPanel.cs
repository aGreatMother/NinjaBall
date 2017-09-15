using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TutorialPanel : MonoBehaviour {

	private Transform pageHolder;
	private int pageIndex;
	private GameObject pageOnScence;
	public GameObject nextButton;
	public GameObject prevButton;
	public string path="tutorial/t";
	// Use this for initialization
	void Awake(){
		
	}
	void Start(){
		pageHolder = transform.GetChild (0);
		pageOnScence = pageHolder.FindChild ("t0").gameObject;
		prevButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextPage(){
		
		pageIndex += 1;
		RefreshPage ();
		if (!LoadPageByIndex (pageIndex + 1))
			nextButton.SetActive (false);

		if (LoadPageByIndex (pageIndex - 1))
			prevButton.SetActive (true);
	}

	public void PrevPage(){

		pageIndex -= 1;
		RefreshPage ();
		if (!LoadPageByIndex (pageIndex - 1))
			prevButton.SetActive (false);

		if(LoadPageByIndex (pageIndex + 1))
			nextButton.SetActive (true);
	}

	private void RefreshPage(){
		GameObject newBorn = Instantiate (LoadPageByIndex (pageIndex))as GameObject;
		Destroy (pageOnScence.gameObject) ;
		newBorn.transform.SetParent (pageHolder,false);
		pageOnScence = newBorn;
	}

	private GameObject LoadPageByIndex(int i){
		GameObject page = Resources.Load (path + i.ToString (), typeof(GameObject))as GameObject;
		return page;
	}

}
