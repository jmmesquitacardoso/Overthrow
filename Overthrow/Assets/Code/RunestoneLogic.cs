using UnityEngine;
using System.Collections;

public class RunestoneLogic : MonoBehaviour {
	
	public Texture2D mouseTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//When the mouse hovers on the enemy, the enemy is highlighted and the cursor changes
	void OnMouseEnter ()
	{
		Cursor.SetCursor (mouseTexture, new Vector2 (0, 0), CursorMode.Auto);
	}
	
	void OnMouseExit ()
	{
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseDown () {
		PlayerBuffs buff = Utils.Instance.GetRandomEnum<PlayerBuffs> ();
		GameObject.Find ("Teste").GetComponent<PlayerControl> ().AddBuff (buff);
	}
}
