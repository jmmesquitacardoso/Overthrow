using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	public Transform target;

	private bool moving = false;

	public float speed = 3.0f;

	private Vector3 targetPosition;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		moving = true;
		Plane missilePlane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (target.position);
		float hitDist = 0.0f;
		
		/*if (missilePlane.Raycast (ray, out hitDist)) {	
			var targetPoint = ray.GetPoint(hitDist);
			targetPosition = ray.GetPoint(hitDist);
			var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			transform.rotation = targetRotation;
		}*/
	
		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
				Destroy(gameObject);
			}
		}
	}
}
