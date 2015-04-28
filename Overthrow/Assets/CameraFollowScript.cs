using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var temp = transform.position;
		temp.x = player.transform.position.x-5;
		temp.z = player.transform.position.z-5;
		transform.position = temp;
	}
}
