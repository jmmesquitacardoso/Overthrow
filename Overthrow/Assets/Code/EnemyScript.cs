using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Texture2D mouseTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		Cursor.SetCursor(mouseTexture, new Vector2(5,0), CursorMode.Auto);
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
}
