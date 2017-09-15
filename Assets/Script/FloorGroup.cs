using UnityEngine;
using System.Collections;

public class FloorGroup : MonoBehaviour {

	private Floor[] floorParts;
	private Transform ball;
	// Use this for initialization
	void Start () {
		floorParts = new Floor[2];
		floorParts = this.GetComponentsInChildren<Floor> ();
		ball = GameObject.Find ("ball").transform;
		SortFloor ();
	}

	void SortFloor(){
		if (floorParts [0].gameObject.transform.position.x > floorParts [1].gameObject.transform.position.x) {
		//get to make a exchange
			Floor temp=floorParts[0];
			floorParts [0] = floorParts [1];
			floorParts [1] = temp;
		}
	
	}
	// Update is called once per frame
	private float distance;
	private int PartsCount;
	//NOTE where the scissor and peaky appear
	public GameObject scissorPrefab;
	public GameObject peakyPrefab;
	//NOTE where the reward appears
	public GameObject rewardPrefab;

	void Update () {
		if (floorParts [0].transform.position.x+floorParts [0].transform.right.x*floorParts [0].longth+3f<ball.transform.position.x) {
			 PartsCount = Random.Range (5, 17);
			distance = 1f+Random.Range (0.4f,0.6f)*floorParts [1].longth;
			//exchange their pos;
			Floor temp=floorParts[0];
			floorParts [0] = floorParts [1];
			floorParts [1] = temp;




			if (Random.Range (0, 5) <4) {
				//scissor appear point

				if (Random.Range (0, 3) < 2) {
					//peaky appear
					Vector3 peakyPoint = floorParts [0].transform.position + floorParts [0].transform.right * floorParts [0].longth
					                    + floorParts [0].transform.right * Random.Range (-2f, distance-2f) + floorParts [0].transform.up * Random.Range (5.5f, 8f);
					;
					GameObject peaky = Instantiate (peakyPrefab);
					peaky.transform.position = peakyPoint;
				} else {
					Vector3 scissorPoint = floorParts [0].transform.position + floorParts [0].transform.right * floorParts [0].longth
						+ floorParts [0].transform.right * Random.Range (0f, distance) + floorParts [0].transform.up * Random.Range (5f, 7f);

					GameObject scissor = Instantiate (scissorPrefab);
					scissor.transform.position = scissorPoint;
				}

			}

			Vector3 newPoint = floorParts [0].transform.position+ floorParts [0].transform.right * floorParts [0].longth
				+ floorParts [0].transform.right * (distance);//start part point
			floorParts [1].PartsAppear (PartsCount, newPoint);

			//make the reward appear
			GameObject reward=Instantiate(rewardPrefab);
			reward.transform.position = newPoint + floorParts [0].transform.right *Random.Range(0f, floorParts [0].longth)+
				floorParts [0].transform.up * Random.Range (5f, 7f);



		}

		
	}
}
