using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	public Vector3 targetPosition;

	private bool moving = false;

	public float speed = 3.0f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		moving = true;
		
		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		
	}
}
