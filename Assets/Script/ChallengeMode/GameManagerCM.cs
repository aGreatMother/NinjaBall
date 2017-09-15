using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerCM : MonoBehaviour {

	public static bool isDead = false;
	// Use this for initialization
	private Transform ball;

	//NOTE call this event to make gameover happen

	[HideInInspector]
	public bool start=false;

	[Header("GameOver")]
	public AudioSource backSound;
	public AudioSource deadSound;

//	[Header("Timer")]
//	public float needTime=0f;
//	public Text timerTxt;
//	private bool needCount=false;

	public Image[] snails;
	public GameObject snailHolder;
	private int snailIndex=0;

	[Header("Pause")]
	public GameObject norPanel;
	public GameObject pausePanel;

	public AudioClip winSound;

	void Start () {
		Camera.main.GetComponent<Animator> ().Play ("move");

		Singleton<MakeRope>.Instance.enabled=false;
		isDead = false;
		ball = GameObject.Find ("ball").transform;
//		if (needTime == 0f)
//			needCount = true;
	}

	// Update is called once per frame

	void Update () {
		
//		if (needCount&&start) {
//
//			needTime += Time.deltaTime;
//			timerTxt.text = needTime.ToString ();
//		}
//		if (!needCount && start) {
//			needTime -= Time.deltaTime;
//			timerTxt.text = Mathf.Round( needTime).ToString ()+" seconds left";
//		}

			
	
//		if (Input.GetKey (KeyCode.Escape)) {
//			norPanel.SetActive (false);
//			pausePanel.SetActive (true);
//			Time.timeScale = 0f;
//		}
	}
	//NOTE where the monster come from hell




	public GameObject gameoverOn;
	//NOTE gameover functions 
	public void GameOver(){
		//for UI move

		//hide the UI on screen
		Singleton<UIfounction>.Instance.wholeUIholder.SetActive(false);
		if (gameOvered)
			return;


		gameoverOn.SetActive (true);


		gameoverOn.GetComponent<RectTransform>().localScale=new Vector2 (1f, 0f);
		gameoverOn.GetComponent<RectTransform>().DOScaleY (1f, 0.3f);
		snailHolder.transform.parent = gameoverOn.transform;
		snailHolder.GetComponent<Animator> ().enabled = true;

		snailHolder.GetComponent<Animator> ().Play ("moveT");

		gameOvered = true;
		ball.GetComponent<Rigidbody2D> ().isKinematic = true;

		if (backSound)
			backSound.Stop ();
		backSound.clip = winSound;
		backSound.loop = false;
		backSound.Play ();
	}



	private bool gameOvered=false;




	public void StartTheGame(){
		Camera.main.GetComponent<Animator> ().enabled = false;
		backSound.enabled = true;
		Singleton<MakeRope>.Instance.enabled=true;
		Singleton<ControlManager>.Instance.enabled = true;
		start = true;
		ball.GetComponent<Rigidbody2D> ().isKinematic = false;
	}

	public void GetSnail(){
		snails [snailIndex].GetComponent<Animator> ().Play ("snailUi");
		snailIndex++;
	}
}
