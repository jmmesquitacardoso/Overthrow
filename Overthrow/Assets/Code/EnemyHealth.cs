using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public Texture2D frame;
	public Rect framePosition;
	public float horizontalDistance;
	public float verticalDistance;
	public Texture2D healthBar;
	public Rect healthBarPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		drawFrame ();
	}

	void drawFrame(){
		framePosition.x = (Screen.width - framePosition.width) / 2;
		framePosition.width = Screen.width * 0.39f;
		framePosition.height = Screen.height * 0.0625f;
		GUI.DrawTexture (framePosition, frame);
	}

}


























