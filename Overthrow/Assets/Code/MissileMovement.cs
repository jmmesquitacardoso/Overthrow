using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	public Transform target;

	private bool moving = false;

	public float speed = 3.0f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moving = true;
		Plane missilePlane = new Plane(Vector3.up, transform.position);
	
		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
			if ((target.position - transform.position).magnitude < 0.1) {
				moving = false;
				Destroy(gameObject);
			}
		}
	}
}
