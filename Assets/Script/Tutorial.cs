using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum TutorialStage {
	aim,
	shot,
	arrow,
	shotOut,
	pre
}

public class Tutorial : MonoBehaviour {
	private TutorialStage stage=TutorialStage.pre;
	public Collider2D ball;
	public Collider2D Note1Area;
	public Collider2D Note2Area;
	public Collider2D Note3Area;
	public Collider2D DeadArea;
	[Header("UI")]
	public GameObject note1;
	public GameObject note2;
	public GameObject note2_1;
	public GameObject note3;
	public GameObject arrow;
	public Transform arrowWorldPos;
	public Transform aimRefer;
	public GameObject noteFinal;
	public GameObject blackBack;
	public RectTransform overPrefab;
	private RectTransform OverShow;
	private Vector3 startPoint;
	private bool passedOnce = false;
	// Use this for initialization
	void Start () {
		startPoint = GameObject.FindObjectOfType<Ball> ().gameObject.transform.position;
	}


	// Update is called once per frame
	void Update () {
		if (OverShow) {
			return;
		}
		if (stage==TutorialStage.pre&&Note1Area.IsTouching (ball)) {
			note1.SetActive (true);
			ball.GetComponent<Rigidbody2D> ().isKinematic = true;
		
			if(Input.GetMouseButtonDown(0))
			stage = TutorialStage.aim;

		}
		if (stage == TutorialStage.aim&&Input.GetMouseButtonDown(0)) {
				note1.SetActive (false);
				note2.SetActive (true);
				arrow.GetComponent<RectTransform> ().position= Camera.main.WorldToScreenPoint(arrowWorldPos.position);
				stage = TutorialStage.arrow;
			Singleton<ControlManager>.Instance.enabled = true;

		}
		if (stage == TutorialStage.arrow ) {
			if (Mathf.Abs (aimRefer.localEulerAngles.z - 170f) < 10f) {
				note2_1.SetActive (true);
				note2.SetActive (false);


				if (Input.GetButtonDown ("shot")) {
					ball.GetComponent<Rigidbody2D> ().isKinematic = false;
					stage = TutorialStage.shotOut;
				}

			} else {
				note2_1.SetActive (false);
				note2.SetActive (true);
			}
			if (stage == TutorialStage.shotOut) {
				note2_1.SetActive( false);
			}

		   
		}
		if (Note2Area.IsTouching (ball)) {
			note3.SetActive (true);
			Time.timeScale = 0.2f;
			note3.GetComponentInChildren<Animator> ().speed = 4.5f;

			if (!Rope2D.exsitOne) {
			//NOTE different from platform to platform
				note3.SetActive (false);
				Time.timeScale = 1f;
			}
		}
		if (Note3Area.IsTouching (ball)) {
			if (passedOnce&&!OverShow) {
				noteFinal.SetActive (false);
				PlayerPrefs.SetInt ("passedTutorial", 1);
				OverShow = Instantiate (overPrefab);
				OverShow.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform, false);

				OverShow.DOScaleY (1f, 0.6f);
				OverShow.DORotate (Vector3.zero, 1.5f);
				blackBack.SetActive (true);
				blackBack.GetComponent<Animator> ().Play ("toBlack");


				return;
			}
			note3.SetActive (false);
			Time.timeScale = 1f;

			Note1Area.enabled = false;
			Note2Area.enabled = false;
			blackBack.SetActive (true);
			StartCoroutine (ReStart ());
		}
		if (DeadArea.IsTouching (ball)) {
			if (!passedOnce)
				SceneManager.LoadScene (1);
			else {
				GameObject ballObj = GameObject.FindObjectOfType<Ball> ().gameObject;
				ballObj.transform.position = startPoint;
				ballObj.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			}
		}
	}

	IEnumerator ReStart(){
		yield return new WaitForSeconds (0.8f);
		blackBack.SetActive (false);
		noteFinal.SetActive (true);
		GameObject ballObj = GameObject.FindObjectOfType<Ball> ().gameObject;
		ballObj.transform.position = startPoint;
		ballObj.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		passedOnce = true;
	}

}
