using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float speed = 3.0f;
	private Vector3 targetPosition, cameraTargetPosition;
	public bool moving = false;
	private Ray ray;
	private RaycastHit hit;
	private Transform currentTarget;
	public Transform elementalMissiles;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		PlayerSkills ();

		MouseMovement ();

		UpdateCurrentTarget ();
	}

	void OnCollisionEnter(Collision collision) {
		if (moving) {
			moving = false;
		}
	}

	void UpdateCurrentTarget() {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out hit))
		{
			currentTarget = hit.transform;
		}
	}

	void PlayerSkills() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			if (currentTarget.tag == "Enemy") {
				elementalMissiles.GetComponent<MissileMovement>().target = currentTarget;
				elementalMissiles.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z+1);
				Debug.Log (currentTarget.name);
				Instantiate(elementalMissiles);
			}
		}
		
		
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			moving = false;
			GetMouseWorldPosition();

			transform.position = targetPosition;
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
			GetMouseWorldPosition();
		}
		
		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
			}
		}
	}

	void GetMouseWorldPosition() {
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
}