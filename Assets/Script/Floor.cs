using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Floor : MonoBehaviour {

	// Use this for initialization
	public float longth=0f;
	private Transform ball;
	void Start () {
		ball = GameObject.Find ("ball").transform;
		count = 5;
		//make the first one appear;
		target = this.transform.position;
		longth = count * gap;
		part1 = (GameObject)Instantiate (partPrefab);
		part1.transform.SetParent (this.transform);
		Vector3 endPoint = target + part1.transform.right * part1.GetComponent<Renderer> ().bounds.extents.x / 2;
		part1.transform.position= endPoint-distance* part1.transform.up;
		//change the color
		rolling = false;

		if (timeToChange)
			part1.GetComponent<FloorPart> ().SetRenderer (newPic);

		timeToChange = !timeToChange;
		part1.transform.DOMove (endPoint,appearTime);

	}

	// Update is called once per frame
	void Update () {
		if (!rolling && part1.GetComponent<Renderer> ().isVisible)
			StartCoroutine (PartsMoveTo (count, target));
			
		}




	static public float distance = 6f;
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.name == "ball") {
			//timeToLeave = true;

		}
	
	}







	public GameObject partPrefab;
	public GameObject squareMonPrefab;//part with a sqare mountain 
	public GameObject hSpuMonPrefab;//part with a high sqare mountain 
	[HideInInspector]
	public GameObject part1;
	private Vector3 target;
	private int count;
	private float appearTime=0.8f;
	private float appearDelay=0.3f;
	public void PartsAppear(int n,Vector3 point){//point is where to start
		
		for (int i = 0; i < this.transform.childCount; i++)
			if(transform.GetChild(i))
				Destroy (transform.GetChild(i).gameObject);

		this.transform.position = point;
		longth = n * gap;

		//make the first one appear;
		target = point;
		count = n;
		part1 = (GameObject)Instantiate (partPrefab);
		part1.transform.SetParent (this.transform);
		Vector3 endPoint = point + part1.transform.right * part1.GetComponent<Renderer> ().bounds.extents.x / 2;
		part1.transform.position= endPoint-distance* part1.transform.up;
		//change the color
	

		if (timeToChange)
			part1.GetComponent<FloorPart> ().SetRenderer (newPic);

		timeToChange = !timeToChange;
		part1.transform.DOMove (endPoint,appearTime);

		rolling = false;

	
	}

	private bool timeToChange=false;
	public Sprite newPic;
	private float gap=2f;//measured by hand
	private bool rolling=false;


	//from the second one!
	IEnumerator PartsMoveTo(int n,Vector3 point){
		//point means the point on the most left
		rolling=true;
		bool haveAmon=false;
		GameObject part=null;
		bool hurry=false;
		for (int i = 1; i < n; i++) {
			if (!hurry&&((i > 1 && ball.position.x > part.transform.position.x-5f)||(i == 1 && ball.position.x > part1.transform.position.x-5f))) {
				appearTime = 0.3f;
				appearDelay = 0f;
				hurry = true;
			}

				
			if (!haveAmon&&i > 5&&n-i>2&& Random.Range (0, 2) == 1 && !hurry) {//the second means the rest parts is more than 3
				if(i>6&&n-i>4&& Random.Range (0, 3) < 2 )//the second means the rest parts is more than 4
					part = (GameObject)Instantiate (hSpuMonPrefab);
				else 
				part = (GameObject)Instantiate (squareMonPrefab);
				haveAmon = true;
				timeToChange = false;
			}
			else
			part = (GameObject)Instantiate (partPrefab);
			part.transform.SetParent (this.transform);
			Vector3 endPoint = point + part.transform.right * part.GetComponent<Renderer> ().bounds.extents.x/2
				+(i * part.transform.right *  gap) ;
			part.transform.position= endPoint-distance* part.transform.up;
			//change the color
			if(timeToChange)
				part.GetComponent<FloorPart> ().SetRenderer(newPic);

			timeToChange = !timeToChange;
			part.transform.DOMove (endPoint,appearTime);
			yield return new WaitForSeconds (appearDelay);
		}

	
	
	}

}
