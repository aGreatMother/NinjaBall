using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Scissors : MonoBehaviour {
		
	public AudioClip cutSound;				//audio which is played when rope is cut
	public GameObject cutFX;				//object which will be instantiated at cut position
	public bool limitCutCount = false;		//limit cuts for whole scene
	public int maxCutCount = 5;				//max count of cuts for whole scene
	public bool limitCutPerObject = false;	//limit cut for per object
	public int maxCutPerObject = 2;			//max count of cuts per object
	public bool newMatPerChunk = true;		//do we create new material for each cut part? used for ropes using linerenderer to make texture scale correspond to rope size

	private bool cutting = false;			//determines if cutting is in process
	private bool cut = true;				//used to limit cut per object
	private int cutCount = 0;				//used to limit cut for whole scene
			
	private Transform ball;


	private bool dead=false;
	private ParticleSystem fireAnim;



	private AudioSource audioSrc;

	private Transform target; //the target of the scissors
	private Animator anim;
	//used to save chain's IDs
	private Hashtable ropeHash = new Hashtable();


	// Use this for initialization
	void Start ()
	{

		audioSrc = GetComponent<AudioSource>();
		ball = GameObject.Find ("ball").transform;
		fireAnim = this.GetComponentInChildren<ParticleSystem> ();


		anim = this.GetComponent<Animator> ();
		if(!limitCutCount)
			maxCutCount = cutCount + 1;
	}


	// Update is called once per frame
	void Update ()
	{
		if (!dead&&target!=null&&!finished) {   
			if (!anim.GetBool ("go"))
				anim.SetBool ("go", true);


			MoveAndRotateToTarget ();

			//position cutter object to touch/click position

			cutting = true;

			//wait little bit before showing trail renderer. used to not draw trail at first click/touch, 
			//because this object jumps from last click/touch position to current click/touch position and it'll look weird
		
		} 
		if (!Rope2D.exsitOne&&moving&&!finished) {
			//randomDir = (Random.Range (0, 2) == 0 ? 1 : (-1)) * Vector3.up;
			finished = true;
			ChangeTriggerRange ();
		}
		if (finished)
			MoveToEdge ();
	
			
		if (ball.position.x > transform.position.x + 10f)
			Destroy (this.gameObject);
	
	}
	private float moveTime =0.3f;
	private float timerForMove;
	private bool moving=false;
	private Vector3 originDirct;

	void MoveAndRotateToTarget(){
		if (!moving) {
			timerForMove = Time.time;
			originDirct = Vector3.up;//get its left...picture problems!!
			moving = true;
		}
		else  {
			
			Vector3 dirct = (target.position - this.transform.position).normalized;
			this.GetComponent<Rigidbody2D>().AddForce(30f*Vector3.Lerp(originDirct,dirct,(Time.time-timerForMove)/moveTime));
			this.transform.localEulerAngles += Vector3.forward * 20f;
		}

	
	}
	//private Vector3 randomDir=Vector3.up;
	void MoveToEdge(){
		this.GetComponent<Rigidbody2D> ().AddForce (10f*Vector3.up);

		this.transform.localEulerAngles += Vector3.forward * 10f;
	}

	private bool finished=false;
	private float workDey=0.2f;
	private float countForWork=0f;
	//called when cutter object exit's from another object's collider
	void ChangeTriggerRange()
	{if (this.GetComponent<CircleCollider2D> ())
			this.GetComponent<CircleCollider2D> ().radius *= 0.8f;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		
		if(Time.time-countForWork>workDey&&cutCount < maxCutCount && cutting && col.transform.parent && col.tag == "rope2D"
			&& col.GetComponent<HingeJoint2D>())
			{
			   //NOTE  cut the rope
			    if (dead)
				 return;
			
				//if limitCutPerObject is set to true, check if we can cut it again
				if(limitCutPerObject)
				{
					var id = col.transform.parent.GetInstanceID(); //get object ID

					if(ropeHash.ContainsKey (id))
					{
						if((int)ropeHash[id] < maxCutPerObject - 1) //rope can be cut again
						{
							cut = true;
							ropeHash[id] = (int)ropeHash[id] + 1;
						}
						else cut = false;
					}
					else
					{
						ropeHash.Add (id,0);
						cut = true;
					}
				}

				//if we can cut rope
				if(!limitCutPerObject || (limitCutPerObject && cut))
				{
					//if audio isn't playing, play cut sound
					if(!audioSrc.isPlaying)
					{
						audioSrc.clip = cutSound;
						audioSrc.Play ();
					}

					col.GetComponent<HingeJoint2D>().enabled = false; //disable cut object's HingeJoint2D component, so it won't be connected to another object
					col.isTrigger = true;
					col.GetComponent<Renderer>().enabled = false;

					//if cutFX object is set (from inspector window), instantiate that object at cut position
					if(cutFX)
					{
					var obj = Instantiate (cutFX) as GameObject;
					obj.transform.position = col.transform.position;
					obj.transform.localEulerAngles = Vector3.forward *( col.transform.localEulerAngles.z - 90f);
						Destroy (obj, 1);
					}

					//if rope has attached "UseLineRenderer" script call its Split function
					if(col.transform.parent && col.transform.parent.GetComponent<UseLineRenderer>())
					{
						col.transform.parent.GetComponent<UseLineRenderer>().Split(col.name, newMatPerChunk);
					
					}


				//when the cut finished!!
				Rope2D.cutted = true;	
			
				finished = true;
				ChangeTriggerRange ();
				Destroy (this.gameObject, 3f);

				cutting = false;
				target = null;
				if(limitCutCount)
						cutCount ++;
				
				}



			}
		
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "floor"&&finished) {
			this.GetComponent<Rigidbody2D> ().isKinematic=true;
			finished = false;
			moving = false;
			cutting = false;
		
			Destroy (this.gameObject,5f);
			this.enabled=false;
		}
	
	}



	public void Burned(){
		if (dead)
			return;//if dead aready, don't move further.

		dead = true;
		this.GetComponent<EnemyScore> ().getScoreAndShowUI ();
		this.GetComponent<Collider2D> ().enabled = false;
		Rigidbody2D rigid = this.gameObject.GetComponent<Rigidbody2D> ();
		rigid.gravityScale = 1f;
		rigid.AddForce (300f*(Vector2)(transform.position - ball.position).normalized);
		BeSliced ();
	}

	private void BeSliced(){
		//NOTE scissor sliced appear
		GameObject siled = GameObject.Instantiate (GameObject.FindObjectOfType<Ball> ().slisePrefab);
		siled.transform.position = ball.position + (this.transform.position - ball.transform.position).normalized * 2.5f;
		siled.transform.up = this.transform.position - ball.transform.position;
		anim.Play ("dead");
		Destroy (this.gameObject, 1f);
		Singleton<SoundManager>.Instance.PlaySwordWave ();
		Singleton<SoundManager>.Instance.DartGetKilled ();
		Destroy (siled, 0.6f);
	}
	//NOTE used to fire it 
//	IEnumerator FireForPeriod(){
//		float firetime = 0.6f;
//		float counter = 0f;
//		while(counter<firetime){
//
//			counter += Time.deltaTime;
//			yield return 0;
//		}
//		fireAnim.Stop ();
//		Destroy (this.gameObject,0.3f);
//	}

	public void GetTarget(Transform init){
		target = init;
		countForWork = Time.time;
		this.GetComponent<Collider2D> ().enabled = true;
		//Debug.Log ("get it"+Time.time);


	}
}
