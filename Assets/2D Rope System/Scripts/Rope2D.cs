using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Rope2D
{
	private Transform objHolder;				//saves chains parent transform
	private GameObject availableChainsHolder;	//saves pooled chains game object
	private PoolingSystem ropePool;				//used for object pooing
	private UseLineRenderer useLinerend = null;
	static public  bool exsitOne=false;
	static public bool cutted=false;
	public void Initialize(GameObject chainObject, int count)
	{
		//if pooled objects holder isn't created, create it
		if(!availableChainsHolder)
		{
			availableChainsHolder = GameObject.Find ("Available Chains Holder");

			if(!availableChainsHolder)
				availableChainsHolder = new GameObject("Available Chains Holder");
		}

		//create pooled objects
		ropePool = new PoolingSystem(chainObject, count, "availableChain", availableChainsHolder.transform);
	}
	



	public bool producing=false;//to make sure can not make another rope when the existing rope is being producing
	//NOTE length of each unit of rope
	private float length=0.925f;
	//create rope with delay
	public IEnumerator CreateRopeWithDelay (GameObject objectsHolder, GameObject chainObject, Transform pointA, Transform pointB,
		                                      bool lockFirstChain, bool lockLastChain, bool connectToA, bool connectToB, 
		                                     bool useLineRenderer, Material ropeMat, float ropeWidth, Transform hook)
	{
		

		//FIXME change the joint linked to Point B 
		//startCreating
		objHolder = objectsHolder.transform;
		producing = true;
		cutted = false;
		//reset variables
		int remainChainCount = 0;
		int chainIndex = 0;
		Quaternion newRotation;
		bool connected = false;
		Transform tempChain = null;

		//calculate how many chains is needed
		remainChainCount = (int)(Vector3.Distance(pointA.position, pointB.position) / (chainObject.GetComponent<Renderer>().bounds.extents.x *length)); 

		useLinerend = null;
		//if useLineRenderer is true, add UseLineRenderer.cs to rope parent, set it material and width
		if(useLineRenderer)
		{
			useLinerend = objectsHolder.AddComponent<UseLineRenderer>();
			useLinerend.ropeMaterial = ropeMat;
			useLinerend.width = ropeWidth;
			useLinerend.AddChain (pointA);

		}

		//while rope isn't connected to end point
	   
		while (!connected)
		{
			Transform chain;

			if(ropePool != null)
				chain = ropePool.Create().transform;	//use pooling system
			else
				chain = ((GameObject)GameObject.Instantiate (chainObject)).transform;	//create chain using standard Instantiate function

			//if useLineRenderer is true, disable chain's renderer
			if (useLineRenderer)
				chain.GetComponent<Renderer>().enabled = false;
			else
				chain.GetComponent<Renderer>().enabled = true;
			
			//positioning and naming chains
			if (!tempChain)//NOTE if this is first chain
			{
				chain.position = pointA.position;
				chain.name = "chain 1";

				chain.GetComponent<Collider2D>().isTrigger = true;

			

				//if connecToA is true, connect pointA to chain

					var joints = pointA.GetComponentsInChildren<FixedJoint2D>();	//get all DistanceJoint2D component from pointA
					FixedJoint2D	distJoint = new FixedJoint2D();

					//iterate through all joints and get free one
					foreach (var joint in joints)
					{
						if(!joint.connectedBody)
							distJoint = joint;
					}
					
					//distJoint.distance = 0.01f;
					distJoint.connectedBody = chain.GetComponent<Rigidbody2D>();
					pointA.GetComponent<Rigidbody2D>().isKinematic = false;

			}
			else //if this isn't first chain
			{
				//position newly created chain next to last chain
				chain.position = tempChain.position + ( (pointB.position - tempChain.position) / remainChainCount );
				chain.name = "chain " + (1 + chainIndex);
				hook.position = chain.position;			
			}
		

			
			//rotate chain object to look at end position
			newRotation = Quaternion.LookRotation(pointA.position - pointB.position, Vector3.up);
			newRotation.x = 0;
			newRotation.y = 0;
			chain.rotation = newRotation;

			if(useLineRenderer)
			{
				if(useLinerend)
					useLinerend.AddChain (chain);
				else 
					break;
			}

			//if at least 1 chain is created
			if(tempChain)
			{
				

				//get last created chain's HingeJoint2D component and connect it to newly created object

					
				var tempChainHinge = tempChain.GetComponent<HingeJoint2D>();
				var midPos = tempChain.position + ((chain.position - tempChain.position).normalized *
					(chainObject.GetComponent<Renderer>().bounds.extents.x));
				tempChainHinge.anchor = tempChain.transform.InverseTransformPoint (midPos);
				tempChainHinge.connectedAnchor = chain.transform.InverseTransformPoint(midPos);
				tempChainHinge.connectedBody = chain.GetComponent<Rigidbody2D>();
				tempChainHinge.enabled = true;

				//if this is second chain
				if(chainIndex == 1)
				{
					//add second hingejoint component to second chain object and connect to first chain
					//it's necessary to swing rope freely around both ends
					var chainHinge = chain.gameObject.AddComponent<HingeJoint2D>();
					chainHinge.connectedBody = tempChain.GetComponent<Rigidbody2D>();

					//set anchor position
					if(!connectToA)
					{
						chainHinge.anchor = new Vector2(-tempChainHinge.anchor.x, -tempChainHinge.anchor.y);
						chainHinge.connectedAnchor = new Vector2(-tempChainHinge.connectedAnchor.x, -tempChainHinge.connectedAnchor.y);
					}
					else
					{
						chainHinge.anchor = chainHinge.transform.InverseTransformPoint (tempChain.position);
					}

					GameObject.DestroyImmediate(tempChainHinge);
				}
			}
			
			//calculate how many chain is needed from last chain position to end position
			remainChainCount = (int) (Vector3.Distance (chain.position, pointB.position) / (chainObject.GetComponent<Renderer>().bounds.extents.x * length));
			
			//if it's last chain, make it isKinematic
			if(remainChainCount < 1)
			{
				if(!tempChain.GetComponent<HingeJoint2D>())
				{
					Debug.LogWarning ("Distance from pointA to pointB is very small, increase distance");
					chain.parent = objectsHolder.transform;
					break;
				}

				//don't limit angle
				var tempChainJoint = tempChain.GetComponent<HingeJoint2D>();
				tempChainJoint.useLimits = false;

				//set anchor position
				if(connectToB)
				{
					tempChainJoint.connectedAnchor = Vector2.zero;
					tempChainJoint.anchor = tempChain.transform.InverseTransformPoint(chain.position);
				}

				chain.GetComponent<Collider2D>().isTrigger = true;



				//if connectToB is true, connect pointB to last chain
				if(connectToB)
				{
					var joints = pointB.GetComponentsInChildren<FixedJoint2D>();	//get all DistanceJoint2D from pointB
					FixedJoint2D	distJoint = new FixedJoint2D();

					//iterate through joints and return free one
					foreach (var joint in joints)
					{
						if(!joint.connectedBody)
							distJoint = joint;
					}

					if(!distJoint)
						distJoint = pointB.gameObject.AddComponent<FixedJoint2D>();

				
					distJoint.connectedBody = chain.GetComponent<Rigidbody2D>();
					pointB.GetComponent<Rigidbody2D>().isKinematic = true;
				}
				else if(lockLastChain) 
					chain.GetComponent<Rigidbody2D>().isKinematic = true;

				GameObject.DestroyImmediate (chain.GetComponent<HingeJoint2D>());	//remove HingeJoint2D component for last chain
				
				connected = true;
			}

			if(objectsHolder)
				chain.parent = objectsHolder.transform; //child this chain into objectsHolder gameobject in hierarchy window			
			else
				break;

			tempChain = chain;//save just instantiated chain in tempChain variable
			
			chainIndex ++;
			//to make the rope appear faster
			count++;
			if (count % 12 == 1)
				yield return new WaitForSeconds (0);
			
		}
		if(useLineRenderer)
		{
			if(useLinerend)
				useLinerend.AddChain (pointB);

		}

		//to let it open
		hook.position = pointB.position;
	
		hook.GetComponent<Hook>().Open();
		Singleton<SoundManager>.Instance.RopeHitFloorPlay ();
		exsitOne = true;
		producing = false;




	}
	int count=0;//for letting the rope appear faster;

	public void Remove()
	{
		if(objHolder)
		{
			//iterate through all HingeJoint2D components, disable them and use pooling system Remove function which doesn't delete object, it makes it invisible
			var hingeHolder = objHolder.GetComponentsInChildren<HingeJoint2D>();
			for(int i = 2; i < hingeHolder.Length - 2; i++)
			{
				hingeHolder[i].enabled = false;
				ropePool.Remove (hingeHolder[i].gameObject);
			}


			//these objects aren't usable for pooling, so we really remove them
			var transformHolder = objHolder.GetComponentsInChildren<Transform>();
			for(int j = 0; j < transformHolder.Length; j++)
			{
				ropePool.RemoveFromList (transformHolder[j].gameObject);
				GameObject.Destroy(transformHolder[j].gameObject);
			}
		}
		ropePool.ClearPolling ();
		exsitOne = false;
	}

	public GameObject GetChainByIndex(int i){
		GameObject chain = ropePool.GetItemByIndex (i);
		return chain;
	}

	public void RemoveOneByPooling(GameObject obj){
		ropePool.Remove (obj);
	
	}

}

