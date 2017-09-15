using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource peakyGetKilled;
	public AudioSource dartGetKilled;
	public AudioSource ropeHitFloor;
	public AudioSource swordWave;


//	private AudioSource[] sounds;

	// Use this for initialization
	void Start () {
//		sounds = Object.FindObjectsOfType<AudioSource> ();
		if (PlayerPrefs.GetInt ("sound") == 0) {

			SoundOn ();
		} else {
			SoundOff ();

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		GameManager.gameOver += GameOverSoundChange;
	}
	void OnDisable(){

		GameManager.gameOver -= GameOverSoundChange;

	}

	public void peakyGetKilledSoundPlay(){
		peakyGetKilled.Play ();
	}

	public void RopeHitFloorPlay(){
		ropeHitFloor.Play ();
	}
	public void DartGetKilled(){
		dartGetKilled.Play ();
	}
	public void PlaySwordWave(){
		swordWave.clip = Resources.Load ("audio/sword/swordW" + Random.Range (1, 4).ToString (), typeof(AudioClip)) as AudioClip;
		swordWave.Play ();
	}

	void GameOverSoundChange(){
		StartCoroutine (DelayDisableSound ());
		}

	IEnumerator DelayDisableSound(){
		yield return new WaitForSeconds (0.8f);
		SoundOff ();
	}
	public void SoundOn(){
		AudioListener.volume = 1f;
//		for (int i = 0; i < sounds.Length;i++) {
//			sounds [i].enabled = true;
//		}
	}
	public void SoundOff(){
		AudioListener.volume = 0f;

//		for (int i = 0; i < sounds.Length;i++) {
//			sounds [i].enabled = false;
//		}
	}

	public void StartSound(){
		if (PlayerPrefs.GetInt ("sound") == 0) {

			SoundOn ();
		} else {
			SoundOff ();

		}
	}
}


