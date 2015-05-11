using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

	public int smooth;
	public float speed = 3.0f;
	private Vector3 targetPosition, cameraTargetPosition;
	public bool moving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			moving = true;
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float hitDist = 0.0f;

			if (playerPlane.Raycast (ray, out hitDist)) {	
				var targetPoint = ray.GetPoint(hitDist);
				targetPosition = ray.GetPoint(hitDist);
				var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				transform.rotation = targetRotation;
			}
		}

		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
			/*cameraTargetPosition = targetPosition;
			cameraTargetPosition.x /= 5;
			cameraTargetPosition.y = Camera.main.transform.position.y;
			cameraTargetPosition.z /= 5;
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraTargetPosition, Time.deltaTime * speed);*/
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
			}
		}

	}

	void OnCollisionEnter(Collision collision) {
		if (moving) {
			moving = false;
		}
	}
}
