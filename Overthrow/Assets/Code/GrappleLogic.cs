using UnityEngine;
using System.Collections;

public class GrappleLogic : MonoBehaviour {

	public Vector3 playerPosition;
	private Vector3 targetPosition;

	public float grappleSpeed = 20f;

	public Quaternion playerRotation;

	// Use this for initialization
	void Start () {
		targetPosition = new Vector3 (playerPosition.x + 7, 1, playerPosition.z + 7);
		var rotation = transform.rotation.eulerAngles;
		rotation.z = 90;
		transform.rotation = Quaternion.Euler (rotation);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * grappleSpeed);
	}

	void OnCollisionEnter(Collision collision) {
		
	}
}
