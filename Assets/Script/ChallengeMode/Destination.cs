using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Destination : MonoBehaviour {
	public Animator light;
	private Transform finalPoint;
	private Transform ball;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name=="ball") {

			ball.DOJump (this.transform.position+this.transform.up,1f,1,1f);
			this.GetComponent<AudioSource> ().Play ();
			StartCoroutine (AfterAWhileThenTurnOffTheLight ());

		}
	}
	// Use this for initialization
	void Start () {
		finalPoint = transform.FindChild ("point");
		ball = Singleton<Ball>.Instance.transform;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator AfterAWhileThenTurnOffTheLight(){
		yield return new WaitForSeconds (1f);
		ball.GetComponent<Collider2D> ().enabled = false;
		if(Vector3.Project((finalPoint.position-ball.position),finalPoint.up).y<9f)
			ball.DOJump (finalPoint.position, -this.transform.up.y*5f, 0, 1f, false);
		else
			ball.DOMove (finalPoint.position,1f, false);
		light.Play ("fade");
		ball.gameObject.SetActive (false);
		this.GetComponent<Animator> ().Play ("des");
	}

	public void GameOverCall(){
		Singleton<GameManagerCM>.Instance.GameOver ();
	}
}
