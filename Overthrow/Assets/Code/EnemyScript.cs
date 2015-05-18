using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Texture2D mouseTexture;

	private Renderer rend;

	private Color originalColor;

	public Vector3 pulledToPosition;

	public bool pull = false;

	public float pullSpeed = 20f;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (pull) {
			transform.position = Vector3.MoveTowards(transform.position, pulledToPosition, Time.deltaTime * pullSpeed);
		}
	}

	//When the mouse hovers on the enemy, the enemy is highlighted and the cursor changes
	void OnMouseEnter()
	{
		Cursor.SetCursor(mouseTexture, new Vector2(5,0), CursorMode.Auto);
		originalColor = rend.material.color;
		rend.material.color = Color.yellow;
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		rend.material.color = originalColor;
	}
}
