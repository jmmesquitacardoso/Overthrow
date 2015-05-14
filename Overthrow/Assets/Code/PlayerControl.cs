using UnityEngine;
using System.Collections;

enum Mode {ARPG, Stealth};

public class PlayerControl : MonoBehaviour {
	
	public float speed = 3.0f;
	public float blinkCooldown = 15;
	public float naturesWrathCooldown = 15;
	private float blinkTimeSpan;
	private float naturesWrathTimeSpan;

	private Vector3 targetPosition, cameraTargetPosition;

	public bool moving = false;

	private Ray ray;

	private RaycastHit hit;
	
	public Transform elementalMissiles;
	private Transform currentTarget;

	private Mode mode;

	// Use this for initialization
	void Start () {
		blinkTimeSpan = Time.time;
		mode = Mode.ARPG;
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
			if (mode == Mode.ARPG) {
				ElementalMissiles();
			} else {
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			if (mode == Mode.ARPG) {
				if (blinkTimeSpan <= Time.time) {
					Blink ();
				}
			} else {
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Debug.Log ("Pressed key 3!");
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			Debug.Log ("Pressed key 4!");
		}

		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Switching mode!");
			if (mode == Mode.ARPG) {
				mode = Mode.Stealth;
			} else {
				mode = Mode.ARPG;
			}
		}
	}

	void ElementalMissiles() {
		if (currentTarget.tag == "Enemy") {
			elementalMissiles.GetComponent<MissileMovement>().target = currentTarget;
			elementalMissiles.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z+1);
			Debug.Log (currentTarget.name);
			Instantiate(elementalMissiles);
		}
	}

	void Blink() {
		moving = false;
		GetMouseWorldPosition();
		transform.position = targetPosition;
		blinkTimeSpan = Time.time + blinkCooldown;
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
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float hitDist = 0.0f;
		var rotation = transform.rotation;
		
		if (playerPlane.Raycast (ray, out hitDist)) {	
			var targetPoint = ray.GetPoint(hitDist);
			targetPosition = ray.GetPoint(hitDist);
			var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			rotation = targetRotation;
			rotation.x = 270;
			Debug.Log ("X Rotation after = " + rotation.x);
			targetRotation = rotation;
			Debug.Log ("X Rotation initial = " + targetRotation);
			transform.rotation = targetRotation;
			Debug.Log ("X Rotation final = " + transform.rotation.x);
		}
	}
}