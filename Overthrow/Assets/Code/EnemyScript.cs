using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Texture2D mouseTexture;
	private Renderer rend;
	private Color originalColor;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
