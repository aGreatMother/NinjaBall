using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
	public static bool isDead = false;
	// Use this for initialization
	private Transform ball;
	public delegate void GameOver();
	//NOTE call this event to make gameover happen
	public static event GameOver gameOver;
	public float maxDown=5f;
	[HideInInspector]
	public bool start=false;

	public Monster monsPrefab = null;



	private float monsAppDis=2.3f;//more than that distance , monster appear and catch that. change with diffculty

	[Header("GameOver")]
	public AudioSource backSound;
	public AudioSource deadSound;

	void Start () {
		Singleton<MakeRope>.Instance.enabled=false;
		isDead = false;
		ball = GameObject.Find ("ball").transform;
		direction = GameObject.Find ("StartFloor").transform;

	}
	
	// Update is called once per frame

	void Update () {

	

		if (!isDead&&(this.transform.position.y - ball.position.y) > maxDown) {
			Debug.Log ("dead");
			gameOver ();
			this.GetComponent<CameraMove> ().enabled = false;
			isDead = true;

		}
		if (!isDead&&((this.transform.position.y - ball.position.y) > monsAppDis)&&
			(!monOnScene||(monOnScene&&(monOnScene.transform.position-ball.position).magnitude>10f))) {
			MakeAMonster ();
		}
		if (StuckTooLong ()) {
			RaycastHit2D hit = Physics2D.Raycast(ball.position,-direction.up,100f,1<<10);
			if (hit.collider && hit.collider.gameObject.GetComponent<FloorPart> ())
				hit.collider.transform.DOMove (hit.collider.transform.position - hit.collider.transform.up * 6f, 1f);
		}
	}
	private Monster monOnScene = null;
	private Transform direction;
	//NOTE where the monster come from hell
	void MakeAMonster(){
		if (monOnScene) {
			if (monOnScene.GetComponent<Renderer> ().isVisible) {
				Destroy (monOnScene.gameObject, 2f);
			}
			else
				Destroy (monOnScene.gameObject, 0f);
		}
		monOnScene = Instantiate (monsPrefab);

		monOnScene.transform.position = ball.position - 4f*direction.up;



	}

	private float stuckLimit=2f;
	private float stuckCount;
	private bool StuckTooLong(){//stuck by a mountain
		RaycastHit2D hit = Physics2D.Raycast(ball.position,direction.right,100f,1<<10);

		if (hit.collider && hit.collider.gameObject.GetComponent<FloorPart> ()&&
			((Vector3)hit.point-ball.position).sqrMagnitude<5f&&
			ball.GetComponent<Rigidbody2D> ().velocity.sqrMagnitude < 15f) {
			stuckCount += Time.deltaTime;
			if (stuckCount > stuckLimit)
				return true;
			else
				return false;
		} else {
			stuckCount = 0f;
			return false;
		}
			
	
	}

	void OnEnable(){
		FloorPart.getScore += AddScore;
		gameOver += UIMove;
		gameOver += SwitchGameOverOn;
		EnemyScore.getScore += AddScore;
	}
	void OnDisable(){
		
		FloorPart.getScore -= AddScore;
		gameOver -= UIMove;
		gameOver -= SwitchGameOverOn;

		EnemyScore.getScore -= AddScore;
	}
	public RectTransform gameoverOn;
	//NOTE gameover functions 
	void UIMove(){
		//for UI move

		//hide the UI on screen
		Singleton<UIfounction>.Instance.wholeUIholder.SetActive(false);
		if (gameOvered)
			return;
		

		gameoverOn.gameObject.SetActive (true);
		gameoverOn.localScale = Vector3.right;
		gameoverOn.localEulerAngles = 350f * Vector3.forward;
		if (score > PlayerPrefs.GetFloat ("highestScore")) {
			PlayerPrefs.SetFloat ("highestScore", score);
			gameoverOn.transform.FindChild ("notePad").GetComponent<Text> ().text = "Your score:" + score.ToString () + "\n" + "New record!!!";
		} else {
			gameoverOn.transform.FindChild ("notePad").GetComponent<Text> ().text = "Your score:" + score.ToString () + "\n" + "Highest score:"
				+PlayerPrefs.GetFloat ("highestScore").ToString();
		
		}

		gameoverOn.DOScaleY (1f, 0.6f);
		gameoverOn.DORotate (Vector3.zero, 1.5f);
	
	}



	private bool gameOvered=false;
	void SwitchGameOverOn(){
		gameOvered = true;
	}

	public void GameOverUIOn(){
		if (gameOvered)
			gameoverOn.gameObject.SetActive (true);
	}

	private int score=0;
	public Text scoreShow = null;
	void AddScore(int s){
		score+=s;
		if(scoreShow)
		scoreShow.text= "SCORE "+score.ToString();
	}
	//call gameover from other class
	public  void GameOverCall(){
		if(!gameOvered)
		gameOver ();
	}

	public void StartTheGame(){
		backSound.enabled = true;
		Singleton<MakeRope>.Instance.enabled=true;
		Singleton<ControlManager>.Instance.enabled = true;
		start = true;
		ball.GetComponent<Rigidbody2D> ().isKinematic = false;
	}
}
