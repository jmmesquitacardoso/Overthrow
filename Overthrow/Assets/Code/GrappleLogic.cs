using UnityEngine;
using System.Collections;

public class GrappleLogic : MonoBehaviour {

	public Vector3 playerPosition;
	private Vector3 targetPosition;

	public float grappleSpeed = 10f;

	public Vector3 playerRotation;

	// Use this for initialization
	void Start () {
		Debug.Log (playerRotation);
		transform.rotation = Quaternion.Euler(playerRotation);
		Debug.Log ("Player z position = " + playerPosition.z + " Y rotation = " + playerRotation.y + " sin = " + Mathf.Sin (playerRotation.y));
		targetPosition = new Vector3 (playerPosition.x + Mathf.Cos(transform.rotation.eulerAngles.y)*20, 1, playerPosition.z + Mathf.Sin(transform.rotation.eulerAngles.y) * 20);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * grappleSpeed);
		var scale = transform.localScale;
		scale.y += 0.2f;
		transform.localScale = scale;
		if ((targetPosition - transform.position).magnitude < 0.1) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		
	}
}
