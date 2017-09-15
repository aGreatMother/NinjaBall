using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
public class UIfounction : MonoBehaviour {
	public GameObject wholeUIholder;
	public GameObject tutorialHolder;
	public GameObject tapToStart;
	public Text hint;
	// Use this for initialization
	void Start () {
		//FIXME do need to
		//PlayerPrefs.SetInt ("passedTutorial", 0);
	//PlayerPrefs.SetInt ("tutorialShowed", 0);

		if (PlayerPrefs.GetInt ("tutorialShowed") == 0&&tutorialHolder&&tutorialHolder && !tutorialHolder.activeSelf) {
			// when player havent see the tutorial!!
				tutorialHolder.SetActive (true);
			return;
		}
		if(tapToStart)
		tapToStart.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (hint)
			hint.text = Singleton<MakeRope>.Instance.hint;
	}
	public void Restart(){
		
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}
	public void Next(){

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex+1);

	}
	public void ToTutorial(){

		SceneManager.LoadScene (1);

	}
	public void ToStartSence(){
		SceneManager.LoadScene (3);
	
	}

	public void TapToStart(){
		if (tutorialHolder.activeSelf)
			tutorialHolder.SetActive (false);
		wholeUIholder.SetActive (true);
		if(Singleton<GameManager>.Instance!=null)
		Singleton<GameManager>.Instance.StartTheGame ();

	}
	public void TapToStartCM(){
		if (tutorialHolder&&tutorialHolder.activeSelf)
			tutorialHolder.SetActive (false);
		wholeUIholder.SetActive (true);
		if(Singleton<GameManagerCM>.Instance!=null)
			Singleton<GameManagerCM>.Instance.StartTheGame ();

	}

	public void BringInTapToStart(){
		if (Singleton<GameManager>.Instance.start)
			return;
		if (!tapToStart.activeSelf)
			tapToStart.SetActive (true);
		PlayerPrefs.SetInt ("tutorialShowed", 1);

	}
	public void Exit(){
		Application.Quit ();
	}

	public void GetIntoTheGame(){

		//TODO  make level Select in it
	
		if (PlayerPrefs.GetInt ("passedTutorial") == 0)
			SceneManager.LoadScene (1);
		else {
			
		}
	}

	public void Pause(){
		Time.timeScale = 0f;
	}

	public void UndoPause(){
		Time.timeScale = 1f;

	}

	public void GotoEndless(){
		SceneManager.LoadScene (2);
	}
	public void GotoLevel(int l){
		SceneManager.LoadScene (l);
	}
}
