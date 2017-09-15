using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;

public class MakeRope : MonoBehaviour {


	  
		public GameObject chainObject;			//object which will be used for creating rope
		

		//first and second points, between thes points will be created rope
		public Transform pointA;
	    public Transform pointB;
	    public Transform ropeStartRefer;
	   
		//used to lock or not lock first and last chain of rope
		public bool lockFirstChain = false;
		public bool lockLastChain = false;

		//used to connect or not rope end points to pointA or pointB
		public bool connectToA = true;
		public bool connectToB = true;


		public bool useLineRenderer = false;		//use or not linerenderer for rope
		public Material ropeMat;					//material of linerenderer
	    
		//public float delay = 0.01f;					//delay between chain creation
		public float ropeWidth = 0.0f;	 //width of linerenderer
	   
		private GameObject chainsHolder;			//object which will hold all chains
		private List<Transform> pointsHolderArray;	//list for holding points
		private Rope2D rope = new Rope2D();
	    private Transform ball;
	    private bool draging = false;    
	    [Header("for ban animation")] 
	  	public GameObject banProfab;
	  	 public float maxDistance=20f;
	   	public float minDistance=1f;
	    
	   [HideInInspector]
		public bool aiming = false;
	   
	    [Header("aiming")]  
	    public Animator scopeAnim;
	    public GameObject aimingBarholder;
	    public GameObject aimingBar;
	    public GameObject shotButton;
	   public float aimSensitivity =2;
	[HideInInspector]
	public string hint;

	[Header("Sounds")]  
	public AudioSource ropeShot = null;
	public AudioSource ropeHit = null;
	public AudioSource ropeCut = null;
	public AudioSource baned = null;

	    
	    void Start()
		{


		shotButton.SetActive (true);
		shotButton.GetComponent<Image> ().enabled = false;
		#if !UNITY_ANDROID

		aimingBarholder.gameObject.SetActive(false);
		#endif

		ball = GameObject.Find ("ball").transform;
		var chainHingeJoint = chainObject.GetComponent<HingeJoint2D>();	//get HingeJoint2D component from chainObject

			//if chain object doesn't have 'HingeJoint2D' component attached, log error and pause game
			if(!chainHingeJoint)
		{	Debug.LogError ("Chain Object Doesn't Have 'DistanceJoint2D' Component Attached");
				Debug.Break ();
			}
			else
				chainHingeJoint.enabled = false;

			rope.Initialize (chainObject,50);	//create rope pool
		}
	    private LeanFinger finger = null;
	    private Vector3 disVectorToCam;
		

	void Update ()
		{
		#if !UNITY_ANDROID
		Vector2 mouthPos=Vector2.zero;
		//NOTE still need some other condition in here
		if (Input.GetMouseButton(0)&&!Rope2D.exsitOne&&!aiming&&!draging) {
			mouthPos = Input.mousePosition;

			if (RectTransformUtility.RectangleContainsScreenPoint (ShotButton.rect,mouthPos))
				return;
			aiming = true;
			//appear the rotateUI
			if(!shotButton.GetComponent<Image> ().enabled )
				shotButton.GetComponent<Image> ().enabled = true;
			shotButton.GetComponent<Button> ().interactable = true;
			shotButton.GetComponent<Animator> ().Play ("appear");

			scopeAnim.transform.up =mouthPos-(Vector2)Camera.main.WorldToScreenPoint( ball.transform.position);

			ropeStartRefer.transform.up=scopeAnim.transform.up;
			scopeAnim.Play("scopeAnim");
		}	

		if (aiming &&Input.GetMouseButton(0)&&!Rope2D.exsitOne&&!draging) {
			mouthPos = Input.mousePosition;
			scopeAnim.transform.up = mouthPos-(Vector2)Camera.main.WorldToScreenPoint( ball.transform.position);
			ropeStartRefer.transform.up=scopeAnim.transform.up;
		}
		if ( aiming&&Input.GetMouseButtonUp(0)&&!Rope2D.exsitOne&&!draging) {
			shotButton.GetComponent<Animator> ().SetBool ("out", true);
			aiming = false;
			scopeAnim.Play ("scopeOut");
		}

	
		#else

		if (LeanTouch.Fingers.Count>0&&!Rope2D.exsitOne&&!aiming&&!draging) {
			finger = LeanTouch.Fingers [0];

			if (RectTransformUtility.RectangleContainsScreenPoint (ShotButton.rect,finger.ScreenPosition))
				return;
			aiming = true;
			//appear the rotateUI
			aimingBarholder.GetComponent<Animator> ().Play("rotateBarAppear");		
			
			if(!shotButton.GetComponent<Image> ().enabled )
			shotButton.GetComponent<Image> ().enabled = true;
			shotButton.GetComponent<Button> ().interactable = true;
			shotButton.GetComponent<Animator> ().Play ("appear");
			scopeAnim.transform.up = aimingBar.transform.up;
			ropeStartRefer.transform.up=aimingBar.transform.up;
			scopeAnim.Play("scopeAnim");
    	}	

		if (aiming &&LeanTouch.Fingers.Count>0&&!Rope2D.exsitOne&&!draging) {
			aimingBar.transform.up=finger.ScreenPosition-(Vector2)(aimingBar.transform.position);
			scopeAnim.transform.up = aimingBar.transform.up;
			ropeStartRefer.transform.up=aimingBar.transform.up;
		}
		if ( aiming&&LeanTouch.Fingers.Count == 0&&!Rope2D.exsitOne&&!draging) {
			aiming = false;
			aimingBarholder.GetComponent<Animator> ().Play("rotateBarOut");
			shotButton.GetComponent<Animator> ().SetBool ("out", true);
			scopeAnim.Play ("scopeOut");
		}

		#endif

		if(Rope2D.exsitOne&&!draging)
			{
			StartCoroutine (DragRopeBack ());
			}
		}
	[Header("hook")]
	public GameObject hook;

	public Transform animPoint;
		//create rope
		void Create()
		{
			
		//if we get cut remain part ,destory that
		if(GameObject.Find("new part"))
			Destroy (GameObject.Find ("new part"));

			if(ropeWidth <= 0.0f)
				ropeWidth = chainObject.GetComponent<Renderer>().bounds.size.x;

			//if connectToA is set to true and pointA doesn't have DistanceJoint2D component yet, add it
			if(connectToA)
			{
			var jointA = pointA.GetComponent<FixedJoint2D>();
				if(!jointA || (jointA && jointA.connectedBody))
				{
				pointA.gameObject.AddComponent<FixedJoint2D>();
					//pointA.gameObject.GetComponent<DistanceJoint2D> ().distance = 0f;
					//pointA.GetComponent<Rigidbody2D>().isKinematic = true;//make the point A still; 
				   
				}

			}

			//if connectToB is set to true and pointB doesn't have FixJoint2D component yet, add it
			if(connectToB)
			{
			var jointB = pointB.GetComponent<FixedJoint2D>();
				if(!jointB || (jointB && jointB.connectedBody))
				{
				pointB.gameObject.AddComponent<FixedJoint2D>();
				//FIXME
				pointB.GetComponent<Rigidbody2D>().isKinematic = true;
				}
			}

			//create "Chains Holder" object, used to make chains children of that object
			chainsHolder = new GameObject("Chains Holder");
		//start the rope shooter anim;
		//show anim

		//appear the hook
		hook.SetActive(true);
		hook.GetComponent<Hook>().Close();
		hook.transform.up = pointB.position - pointA.position;
		ropeShot.Play ();
		StartCoroutine(rope.CreateRopeWithDelay(chainsHolder, chainObject, pointA, pointB, lockFirstChain, lockLastChain, connectToA, connectToB, useLineRenderer, ropeMat, ropeWidth, hook.transform));


	
	}
	[Header("drag")]
	public int dragForce = 1000;
     public  IEnumerator  DragRopeBack(){
		int i =0;
		GameObject	formerOne, latterOne; 
		draging = true;
		while( !cut&&!Rope2D.cutted&&rope.GetChainByIndex (i) &&rope.GetChainByIndex (i + 1)){
			//TODO in the tutorial part rope dorging process can be freezed

			formerOne = rope.GetChainByIndex (i);
			latterOne = rope.GetChainByIndex (i + 1);
			pointA.gameObject.GetComponent<Rigidbody2D> ().AddForce ((latterOne.transform.position - formerOne.transform.position).normalized*dragForce);

			if (formerOne.GetComponent<HingeJoint2D> ())
				formerOne.GetComponent<HingeJoint2D> ().enabled = false;
			if (formerOne.GetComponent<Renderer> ())
				formerOne.GetComponent<Renderer> ().enabled = false;
			if(	formerOne.GetComponent<Collider2D> ())
				formerOne.GetComponent<Collider2D> ().isTrigger = true;
//
			if (formerOne.transform.parent && formerOne.transform.parent.GetComponent<UseLineRenderer> ())
				formerOne.transform.parent.GetComponent<UseLineRenderer> ().RemoveSingleChain (formerOne.name);
           

             rope.RemoveOneByPooling(formerOne);

//			if ((pointA.position- latterOne.transform.position).sqrMagnitude > 6f)
//				break;


			pointA.GetComponent<FixedJoint2D>().connectedBody=latterOne.GetComponent<Rigidbody2D>();

			i++;
			if (i % 2 == 1)
				yield return new WaitForSeconds (0f);
		}
		pointB.parent = null;
		rope.Remove ();
		if (!Rope2D.cutted)
			hook.SetActive( false);
		draging = false;
	
	}



	void OnEnable(){
		GameManager.gameOver += OnBallDead;
	}

	void OnDisable(){
		GameManager.gameOver -= OnBallDead;
	}

	void OnBallDead(){
		this.enabled = false;
	}
	private bool cut=false;
	public void ShotOrCut(){

		
		if (Rope2D.exsitOne) {//cut the exsiting rope
			cut = true;
			shotButton.GetComponent<Animator> ().SetBool("out",true);
			shotButton.GetComponent<Button> ().interactable = false;
			ropeCut.Play ();
			aiming = false;
			pointB.parent = null;

		}

		if (!Rope2D.exsitOne&&!rope.producing&&aiming) {//make a new rope
			cut = false;
			shotButton.GetComponent<Animator> ().PlayInFixedTime ("shotToCut");

			RaycastHit2D hit = Physics2D.Raycast(ropeStartRefer.position,ropeStartRefer.up,100f,LayerMask.GetMask("acceptRay"));
			if(hit.collider&&hit.collider.gameObject.tag=="iceFloor")
				hint="note: You can't shot on blue bricks!";


			if (hit.collider&&hit.collider.gameObject.tag=="floor"
				&&!hit.collider.isTrigger
				&&((hit.point-(Vector2)pointA.position).magnitude<maxDistance)&&(hit.point-(Vector2)pointA.position).magnitude>minDistance) {
				hint="";
				//NOTE make the hood move to the raycast point and creat the rope
				pointB.position = (Vector3)hit.point+(pointA.position-(Vector3)hit.point).normalized*0.3f;
				pointB.parent = hit.collider.gameObject.transform;
				Create ();
				aimingBarholder.GetComponent<Animator> ().PlayInFixedTime("rotateBarOut");
				scopeAnim.PlayInFixedTime ("scopeOut");
			}
			else {
				Transform banned = Instantiate (banProfab).transform;
				banned.up = ropeStartRefer.up;
				banned.position = ball.position+ropeStartRefer.up*2f;
				Destroy (banned.gameObject, 0.8f);
				aimingBarholder.GetComponent<Animator> ().PlayInFixedTime("rotateBarOut");
				scopeAnim.PlayInFixedTime ("scopeOut");
				baned.Play ();
			}

		}

	}
		

}
