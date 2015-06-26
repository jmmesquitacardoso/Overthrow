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
		if (Vector3.Distance (GameObject.Find ("Player").transform.position, transform.position) <= 40) {
			GameObject.Find ("Player").GetComponent<PlayerControl> ().hoveringShrine = true;
			Cursor.SetCursor (mouseTexture, new Vector2 (0, 0), CursorMode.Auto);
		}
	}
	
	void OnMouseExit ()
	{
		GameObject.Find ("Player").GetComponent<PlayerControl> ().hoveringShrine = false;
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseDown () {
		if (Vector3.Distance (GameObject.Find ("Player").transform.position, transform.position) <= 40) {
			PlayerBuffs buff = Utils.Instance.GetRandomEnum<PlayerBuffs> ();
			GameObject.Find ("Player").GetComponent<PlayerControl> ().AddBuff (buff);
		}
	}
}
