using UnityEngine;
using System.Collections;

public class RandomAlphaAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<SpriteRenderer> ();
		minAlpha=Random.Range ( 0.7f, 0.8f);
		maxAlpha = Random.Range (0.8f,1.0f);
		StartCoroutine (DelayForChangeAlpha ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float delay=0.4f;
	IEnumerator DelayForChangeAlpha()
	{
		while (delay>=-1f) {
			delay-=Time.deltaTime;
			if(delay<=0f)
			{
				StartCoroutine(RandomAlpha() );
				StopCoroutine (DelayForChangeAlpha ());
				break;
			}
			yield return 0;
		
		}
	}



	public float changeSpeed=0.2f;
	//float originYscale;
	float maxAlpha;
	bool grow=false;
	float minAlpha;
	SpriteRenderer sprite;
	IEnumerator RandomAlpha()
	{
		
		while ( sprite.color.a>minAlpha ){
			if(sprite!=null)
			{Color currentColor=this.sprite.color;
			currentColor.a-=Time.deltaTime * changeSpeed;
				sprite.color=currentColor;}
			//			transform.localScale -= Time.deltaTime * changeYscaleSpeed * Vector3.up;
			//Ypos = sprite.sprite.border.y / 2;
			//transform.position -= transform.right * Ypos;
			yield return 0;
			
		} 
		if (sprite.color.a<=minAlpha  )
			grow = true;
		if(grow)
		while (sprite.color.a<maxAlpha  ){
			if(sprite!=null)
			{Color currentColor=this.sprite.color;
			currentColor.a+=Time.deltaTime * changeSpeed;
				sprite.color=currentColor;}
			yield return 1;
			
		}
		
		if (grow && sprite.color.a>=maxAlpha) {
			grow=false;
			Color currentColor=this.sprite.color;
			currentColor.a=maxAlpha;
			sprite.color=currentColor;
			minAlpha=Random.Range ( 0.2f, 0.3f);
			maxAlpha = Random.Range (0.8f,1.0f);
			StartCoroutine(RandomAlpha());
		}
		
	}




}
