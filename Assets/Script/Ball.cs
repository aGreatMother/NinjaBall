using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	private Rigidbody2D rigid;
	private Transform faceTransfrom;
	private Animator faceAnim;
	private AudioSource hitAudio=null;
	private AudioSource maxSpeedSound=null;
	public AudioSource superEnd;
	public AudioSource backMusic;
	public AudioClip norBackMusic;
	public AudioClip supBackMusic;

	[Header("slise")]
	public float fireSpeed = 100f;
	public Text mode=null;
	public Image speedBar=null;
	public Color fireColor=Color.white;
	public float fireOnTime=3f;
	public GameObject fireOnUI;
	public GameObject slisePrefab;


	[HideInInspector]
	public static bool fireOn = false;
	// Use this for initialization
	void Start () {
		//the sounds
		hitAudio = this.GetComponent<AudioSource> ();
		maxSpeedSound= this.transform.FindChild ("StartPointRefer").GetComponent<AudioSource> ();
	   //end of the sound
		rigid = this.GetComponent<Rigidbody2D> ();
		fireOn = false;
		faceTransfrom = transform.FindChild ("face");
		faceAnim = faceTransfrom.gameObject.GetComponent<Animator> ();
		fireOnUI.transform.localScale = new Vector2 (0.5f, 0.5f);

	}	
	void OnEnable(){
		GameManager.gameOver += Dead;
	}
	void OnDisable(){
		GameManager.gameOver -= Dead;
	}
	
	// Update is called once per frame
	void Update () {
		if (rigid.velocity.x > 0.05f) {
			faceTransfrom.localScale = new Vector3 (1f, 1f, 0f);
			faceTransfrom.right = rigid.velocity;

		} else if(rigid.velocity.x <- 0.05f) {
			faceTransfrom.localScale = new Vector3 (-1f,1f, 0f);
			faceTransfrom.right = -rigid.velocity;


		}
		if(fireOnUI.activeSelf)
		fireOnUI.transform.position = Camera.main.WorldToScreenPoint (transform.position + Vector3.up * 1.5f);
	   
//		NOTE if ball can be fired by speed
//		if (!fireOn && rigid.velocity.sqrMagnitude > fireSpeed) {
//			StartCoroutine (FireOn ());
//		} 
//
//		if (!fireOn) {
//			speedBar.fillAmount = rigid.velocity.sqrMagnitude / fireSpeed;
//			speedBar.color = Color.Lerp (Color.white, fireColor, speedBar.fillAmount);
//		}

	}

	//Control the fire
	private float fireTimeCount=0f;
	IEnumerator FireOn(){
		float timeSoFar = backMusic.time;
		backMusic.time = timeSoFar;
		backMusic.clip = supBackMusic;
		backMusic.Play ();
		maxSpeedSound.Play ();
		speedBar.color = Color.red;
		mode.text="FIRE";
		fireOn = true;
		faceAnim.SetBool ("angry", true);
		fireTimeCount = fireOnTime;
		while(fireTimeCount>0.75f){
			fireTimeCount -= Time.deltaTime;
			speedBar.fillAmount =( fireTimeCount-0.75f) / fireOnTime;
			//check this for every 0.4 second.
			yield return 0;
		}
		timeSoFar = backMusic.time;
		backMusic.clip = norBackMusic;
		backMusic.time = timeSoFar;
		backMusic.Play ();
		superEnd.Play ();
		faceAnim.SetBool ("angry", false);
		fireOn = false;
		fireOnUI.SetActive (false);
		//mode.text="SPEED";
	}
	public void Dead(){
		faceAnim.PlayInFixedTime ("dead");

		this.GetComponent<Animator> ().PlayInFixedTime ("dead");
	}

	public void Attacked(){
		this.GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	void OnTriggerEnter2D (Collider2D other)
	{

		if (other.gameObject.tag == "scissor"&&Ball.fireOn) {
			other.gameObject.GetComponent<Scissors> ().Burned ();

		}



	}
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "floor"&&!hitAudio.isPlaying&&other.relativeVelocity.magnitude>6) {
			hitAudio.volume =other.relativeVelocity.magnitude / 10;
			hitAudio.Play ();
		}
	}

	public void StartFireOn(){
		if (!fireOnUI.activeSelf)
			fireOnUI.SetActive (true);
		if (!fireOn)
			StartCoroutine (FireOn ());
		else {
			fireTimeCount = fireOnTime;
		
		}
	}
}
