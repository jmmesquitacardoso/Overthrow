using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public GameObject enemy;
	public Texture2D frame;
	public Rect framePosition;
	public float horizontalDistance;
	public float verticalDistance;
	public float width;
	public float height;
	public Texture2D healthBar;
	public Rect healthBarPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var wantedPos = Camera.main.WorldToViewportPoint (enemy.transform.position);
		transform.position = wantedPos; 
	}

	void OnGUI(){
		drawFrame ();
		drawBar ();
	}

	void drawFrame(){
		Vector3 screenPosition =
			Camera.current.WorldToScreenPoint(enemy.transform.position);// gets screen position.
		screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
		framePosition = new Rect(screenPosition.x  - 50,
		                     screenPosition.y  - 50, 100, 24);// makes a rect centered at the player ( 100x24 )
		GUI.DrawTexture (framePosition, frame);
	}

	void drawBar(){
		healthBarPosition.x = framePosition.x + framePosition.width * horizontalDistance;
		healthBarPosition.y = framePosition.y + framePosition.height * verticalDistance;
		healthBarPosition.width = framePosition.width * width;
		healthBarPosition.height = framePosition.height * height;

		GUI.DrawTexture (healthBarPosition, healthBar);
	}
}


























