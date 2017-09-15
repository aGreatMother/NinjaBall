using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class MakeAbstractMap : MonoBehaviour {

	private List<Transform> mapBricks;
	private List<Transform> movingBricks;
	private List<RectTransform> movingMapEl;
	Vector3 middlePoint;
	public float length=100f;

	public Image image;
	public Image ballOnMap;
	public Image flag;
	public Color floorColor;
	public Color iceFloorColor;
	private float scale = 1f;
	private Transform ballPos;
	// Use this for initialization
	void Start () {
		ballPos = Singleton<Ball>.Instance.transform;

		InitMap ();
	}
	
	// Update is called once per frame
	void Update () {
		//update the moving brikes pos
		foreach (Transform tr in movingBricks) {
			
				
				
			RectTransform recPos = movingMapEl [movingBricks.IndexOf (tr)];
			recPos.anchoredPosition = (Vector2)( tr.position-middlePoint) * scale;
			recPos.localEulerAngles = tr.eulerAngles;
			if (tr.gameObject.tag == "snail" && tr.GetComponent<Collider2D> () && tr.GetComponent<Collider2D> ().enabled == false)
				recPos.GetComponent<Image> ().enabled = false;
			if(tr.gameObject.activeSelf==false)
				recPos.GetComponent<Image> ().enabled = false;

		}
	}


	void InitMap(){
		mapBricks = new List<Transform> ();
		movingBricks = new List<Transform> ();
		movingMapEl = new List<RectTransform> ();

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("floor")) {
			mapBricks.Add (obj.transform);
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("iceFloor")) {
			mapBricks.Add (obj.transform);
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("snail")) {
			mapBricks.Add (obj.transform);
		}
		mapBricks.Add (GameObject.Find("des").transform);
		Vector3 firPos = mapBricks[0].position;
		float maxX=firPos.x, minX=firPos.x, maxY=firPos.y, minY=firPos.y;
		foreach (Transform tr in mapBricks) {
			if (tr == null)
				return;

			if (tr.position.x > maxX)
				maxX = tr.position.x;
			if (tr.position.x < minX)
				minX = tr.position.x;
			if (tr.position.y > maxY)
				maxY = tr.position.y;
			if (tr.position.y < maxY)
				minY = tr.position.y;
		
		}
		middlePoint = new Vector3 ((maxX + minX) / 2f, (minY + maxY) / 2f, 0f);
		scale = Mathf.Abs(maxX - minX)/length;

		foreach (Transform tr in mapBricks) {
			Image im = GameObject.Instantiate (image).GetComponent<Image> () as Image;
			im.transform.SetParent (this.transform, false);
			RectTransform recPos = im.GetComponent<RectTransform> ();
			recPos.anchoredPosition = (Vector2)( tr.position-middlePoint) * scale;
			recPos.localEulerAngles = tr.eulerAngles;

			recPos.sizeDelta = (Vector2)tr.lossyScale*scale;
			if (tr.gameObject.tag == "floor")
				im.color = floorColor;
			if (tr.gameObject.tag == "iceFloor")
				im.color = iceFloorColor;
			if(tr.gameObject.tag=="snail")
			{im.color = Color.green;
				im.sprite = ballOnMap.sprite;
				im.transform.localScale *= 3.5f;
			}
			if(tr.gameObject.name=="des")
			{im.color = Color.red;
				im.sprite = flag.sprite;
				im.transform.localScale *= 2f;
			}
			if (tr.GetComponent<Animator> () || tr.GetComponentInParent<Animator> ()) {
				movingBricks.Add (tr);
				movingMapEl.Add (recPos);

			}
		}
		Image ballOn = GameObject.Instantiate (ballOnMap).GetComponent<Image> () as Image;
		ballOn.transform.SetParent (this.transform, false);


		RectTransform recPosb = ballOn.GetComponent<RectTransform> ();
		recPosb.anchoredPosition = (Vector2)( ballPos.position-middlePoint) * scale;
		recPosb.localEulerAngles = ballPos.eulerAngles;
		recPosb.sizeDelta = (Vector2)ballPos.lossyScale*scale*3.5f;





		movingBricks.Add (ballPos);
			movingMapEl.Add (recPosb);



	
	}




}
