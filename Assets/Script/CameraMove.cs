using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class CameraMove : MonoBehaviour {

	public Transform roofPrefer;
	public Transform groundPrefer;
	private Vector3 moveDirct;
	private Transform target;

	// Use this for initialization
	void Start () {
		this.transform.position = Vector3.forward*this.transform.position.z+(roofPrefer.position + groundPrefer.position) / 2f+moveDirct*5f;
		moveDirct = roofPrefer.right;
		target = GameObject.Find ("ball").transform;

	}
	
	// Update is called once per frame

	void Update () {
		
			this.transform.position =this.transform.position.z*Vector3.forward+Vector3.Project((Vector2)(target.position),moveDirct)
			+moveDirct*2.5f;




}
	void OnEnable(){
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("tutorial"))
			return;
		GameManager.gameOver +=RotateToHorizontal;
	}

	void OnDisable(){
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("tutorial"))
			return;
		GameManager.gameOver -=RotateToHorizontal ;
	}
	void RotateToHorizontal(){
		this.transform.DOLocalRotate (350f * Vector3.forward, 1.5f);
	}


}