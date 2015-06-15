using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	
	public GameObject enemy;
	public EnemyAI enemyAi;
	private Rect framePosition;
	public float horizontalDistance;
	public float verticalDistance;
	public float width;
	public float height;
	public Texture2D healthBar;
	private Rect healthBarPosition;
	public float healthPercentage;
	// Use this for initialization
	void Start () {
		healthPercentage = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (healthPercentage >= 0) {
			healthPercentage = (float)enemyAi.health / (float)enemyAi.maxHealth;
		} else {
			Destroy(gameObject);
		}
	}

	void OnGUI(){
		drawBar ();
	}

	void drawBar(){
		Vector3 screenPosition =
			Camera.current.WorldToScreenPoint(enemy.transform.position);// gets screen position.
		screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
		framePosition = new Rect(screenPosition.x  - 50,
		                         screenPosition.y  - 50, 100, 24);// makes a rect centered at the player ( 100x24 )
		healthBarPosition.x = framePosition.x + framePosition.width * horizontalDistance;
		healthBarPosition.y = framePosition.y + framePosition.height * verticalDistance;
		healthBarPosition.width = framePosition.width * width * healthPercentage;
		healthBarPosition.height = framePosition.height * height;

		GUI.DrawTexture (healthBarPosition, healthBar);
	}
}


























