using UnityEngine;
using System.Collections;

public class OnPlayClick : MonoBehaviour {

	public void OnMouseUp() {
		Debug.Log ("carrega");
		Application.LoadLevel(1);

	}
}
