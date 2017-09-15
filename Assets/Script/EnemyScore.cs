using UnityEngine;
using System.Collections;

public class EnemyScore : MonoBehaviour {
	public int score;
	public delegate void GetScore(int s);
	public static event GetScore getScore;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private GameObject scoreShow;
	private bool scoreGot=false;
	public void getScoreAndShowUI(){
		if (!scoreGot) {
			getScore (score);
			scoreGot = true;
		}


		if(scoreShow==null)
		scoreShow = GameObject.Instantiate (scoreShowPrefab);
		
		scoreShow.transform.position =new Vector3(transform.position.x,transform.position.y,scoreShow.transform.position.z);
		Destroy (scoreShow, 1f);
	}

	public GameObject scoreShowPrefab = null;

}
