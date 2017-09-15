using UnityEngine;
using System.Collections;
using DG.Tweening;
public class FloorPart : MonoBehaviour {

	private SpriteRenderer render;
	private GameObject ball;
	private Animator ChildAnim;
	public int score=1;
	public delegate void GetScore(int s);
	public static event GetScore getScore;
	private EdgeCollider2D childColl;
	private Collider2D ballColl;
	// Use this for initialization
	void Start () {
		ball = GameObject.Find ("ball");
		ballColl = ball.GetComponent<Collider2D> ();
		ChildAnim = this.GetComponentInChildren<Animator> ();
		childColl = this.GetComponentInChildren<EdgeCollider2D> ();

	}
	void Awake(){
		
	
		render = this.GetComponent<SpriteRenderer> ();

		//render.sprite= Resources.Load ("dm" + Random.Range (1, 10).ToString (),typeof(Sprite)) as Sprite;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ball.transform.position.x > (this.transform.position + this.transform.right * render.bounds.extents.x*2f).x)
			this.transform.DOMove (this.transform.position - this.transform.up * 6f, 1f);

		if (!firstTimeHited && childColl.IsTouching (ballColl)) {
			//NOTE when ball hit floor up and need get score;
			firstTimeHited = true;
			getScore (score);

			if (!ChildAnim.GetBool ("appear"))
				ChildAnim.SetBool ("appear", true);
		}

	}
	private bool firstTimeHited=false;
	public Renderer childRender;
	public Color yellow;


	public void SetColor(Color color){
		render.material.color = color;

	}

	public void SetRenderer(Sprite sprite){
		render.sprite = sprite;
	}

}
