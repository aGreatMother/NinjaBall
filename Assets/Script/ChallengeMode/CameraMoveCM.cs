using UnityEngine;
using System.Collections;

public class CameraMoveCM : MonoBehaviour {


	private Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("ball").transform;


	}

	// Update is called once per frame

	void Update () {

		this.transform.position =this.transform.position.z*Vector3.forward+(Vector3)(target.position)+Vector3.right*2.5f+Vector3.up*1.5f;




	}



}
