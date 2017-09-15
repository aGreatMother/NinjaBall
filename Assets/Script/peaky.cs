using UnityEngine;
using System.Collections;
using DG.Tweening;
public class peaky : MonoBehaviour {

	private Animator anim;
	private bool dead=false;
	private float sensitiveDis = 25f;
	private Transform  ballLocation;
	private AudioSource peakSound;
	// Use this for initialization



	void Start () {
		anim=this.GetComponent<Animator>();
		ballLocation = GameObject.Find ("ball").transform;
		peakSound = this.GetComponent<AudioSource> ();
	}




	void Update () {
		if (!dead&&!anim.GetBool ("on") && ((Vector2)(ballLocation.position - this.transform.position)).sqrMagnitude <= sensitiveDis) {
			peakSound.Play ();
			anim.SetBool ("on", true);
		}


	}
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "ball") {
			if (!Ball.fireOn) {
				if (dead)
					return;
				// fire is not on, peaky is deadly
				other.GetComponent<Ball> ().Attacked ();
				Singleton< GameManager>.Instance.GameOverCall ();
			} else {
				if (dead)
					return;
				
				this.GetComponent<EnemyScore> ().getScoreAndShowUI ();
				dead = true;
				anim.SetBool ("on", false);
				Rigidbody2D rigid = this.gameObject.GetComponent<Rigidbody2D> ();
				rigid.gravityScale = 1f;
				rigid.AddForce (300f*(Vector2)(transform.position - ballLocation.position).normalized);
				this.GetComponent<Collider2D> ().enabled = false;
				BeSliced ();
			}

		}
	}
	private void BeSliced(){
		//NOTE peaky sliced appear
		GameObject siled = GameObject.Instantiate (GameObject.FindObjectOfType<Ball> ().slisePrefab);
		siled.transform.position = ballLocation.position + (this.transform.position - ballLocation.transform.position).normalized * 2.5f;
		siled.transform.up = this.transform.position - ballLocation.transform.position;
		Destroy (siled, 0.5f);
		Singleton<SoundManager>.Instance.PlaySwordWave ();
		Singleton<SoundManager>.Instance.peakyGetKilledSoundPlay ();
		anim.Play ("dead");
		Destroy (this.gameObject, 1f);
	}

}
