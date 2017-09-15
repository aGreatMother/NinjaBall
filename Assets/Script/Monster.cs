using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Monster : MonoBehaviour {

	// Use this for initialization
	private Transform ball;
	private Rigidbody2D rigid;
	private Animator anim;
	private Renderer render;
	private float attachDistance=2.3f;
	private bool attach = false;
	public GameObject hellPrefab;

	void Start () {
		ball = GameObject.Find ("ball").transform;
		rigid = this.GetComponent<Rigidbody2D> ();
		anim = this.GetComponent<Animator> ();
		render = this.GetComponent<Renderer> ();
		if (hellPrefab) {// only be assigned 
			GameObject helllight = Instantiate (hellPrefab);
			helllight.transform.position = this.transform.position + 3f * helllight.transform.up;
			Destroy (helllight, 1f);
		}
		rigid.AddForce (30f * (ball.position - this.transform.position).normalized);
		StartCoroutine (ChaseAfterDelay (2.2f));
		//rigid.AddForce (5f * (ball.position - this.transform.position).normalized);
	}
	

	// Update is called once per frame
	void Update () {
		  
		this.transform.up = ball.position - this.transform.position;

		if (!attach && (ball.position - this.transform.position).magnitude < attachDistance) {
			StopAllCoroutines ();
			StartCoroutine (MoveToBall ());
			if(!this.GetComponent<Collider2D> ().isTrigger )
				this.GetComponent<Collider2D> ().isTrigger = true;
		}
			
	}


	IEnumerator MoveToBall(){
		if (!anim.GetBool ("attack"))
			anim.SetBool ("attack", true);
		attach = true;
		float during = 1f;
		float startTime = Time.time;
		while ((this.transform.position - ball.position).sqrMagnitude > 0.03f) {
			this.transform.position = Vector3.Lerp (this.transform.position, ball.position, (Time.time - startTime) / during);
			this.transform.up = ball.position - this.transform.position;
			yield return new WaitForSeconds (0f);
		}
		attach = false;
		this.rigid.isKinematic = true;
		ball.GetComponent<Ball> ().Attacked ();
		Singleton<GameManager>.Instance.GameOverCall ();
	
	}

	private IEnumerator ChaseAfterDelay(float time){
		Vector3 lastPos = ball.position;
		yield return new WaitForSeconds (time);
		while ((ball.position - this.transform.position).magnitude > attachDistance) {
			//chasing 
			rigid.velocity=Vector2.zero;
			rigid.AddForce (160f * (lastPos - this.transform.position).normalized);
			lastPos = ball.position;


			yield return new WaitForSeconds (time);
		}

		
	}

	public AudioSource movingSound;
	public AudioSource eatSound;

	public void MovingSound(){
		if (render.isVisible) {
			movingSound.Play ();
		}
	}

}
