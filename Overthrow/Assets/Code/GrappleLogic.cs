using UnityEngine;
using System.Collections;

public class GrappleLogic : MonoBehaviour {

	public Vector3 playerPosition;
	public Vector3 targetPosition;

	public float grappleSpeed = 10f;

	public Vector3 playerRotation;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(playerRotation);
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
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<EnemyScript>().pulledToPosition = playerPosition;
			collision.gameObject.GetComponent<EnemyScript>().pull = true;
		}
	}
}
