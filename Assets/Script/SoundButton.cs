using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SoundButton : MonoBehaviour {

	private Image image;
	private Text text;
	public Sprite soundOn;
	public Sprite soundOff;
	// Use this for initialization
	void Start () {
		image = this.GetComponent < Image> ();
		text = this.GetComponentInChildren<Text> ();



		if (PlayerPrefs.GetInt ("sound") == 0) {
			image.sprite = soundOn;
			text.text = "sound:on";

		} else {
			image.sprite = soundOff;
			text.text = "sound:off";

		}

	}
	public void ChangeSoundOption(){
		if (PlayerPrefs.GetInt ("sound") == 1) {
			PlayerPrefs.SetInt ("sound", 0);
		}
		else {
			PlayerPrefs.SetInt ("sound", 1);
		}
		if (PlayerPrefs.GetInt ("sound") == 0) {
			image.sprite = soundOn;
			text.text = "sound:on";
			Singleton<SoundManager>.Instance.SoundOn();
		} else {
			Singleton<SoundManager>.Instance.SoundOff ();
			image.sprite = soundOff;
			text.text = "sound:off";
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
