using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float speed = 3.0f;
	private Vector3 targetPosition, cameraTargetPosition;
	public bool moving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		PlayerSkills ();

		MouseMovement ();

	}

	void OnCollisionEnter(Collision collision) {
		if (moving) {
			moving = false;
		}
	}

	void PlayerSkills() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Debug.Log ("Pressed key 1!");
		}
		
		
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Debug.Log ("Pressed key 2!");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Debug.Log ("Pressed key 3!");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Debug.Log ("Pressed key 4!");
		}
	}

	void MouseMovement() {
		if (Input.GetMouseButtonDown (0)) {
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
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
			}
		}
	}
}
