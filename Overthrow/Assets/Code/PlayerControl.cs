using UnityEngine;
using System.Collections;

enum Mode {ARPG, Stealth};

public class PlayerControl : MonoBehaviour {
	
	public float speed = 3.0f;
	public float blinkCooldown = 15f;
	public float naturesWrathCooldown = 15f;
	public float attackSpeed = 1f;
	public float dodgeChance = 15f;
	public float critChance = 15f;
	private float blinkTimeSpan;
	private float naturesWrathTimeSpan;
	private float globalCooldown;
	private float globalCooldownTimeSpan;

	public int health = 400;
	public int mana = 0200;
	public int strength = 100;
	public int attackPower = 1000;
	public int criticalHitDamage = 1;

	private Animator anim;

	private Vector3 targetPosition;

	private bool moving = false;

	private Ray ray;

	private RaycastHit hit;
	
	public Transform elementalMissiles;
	public Transform grapple;
	private Transform currentTarget;

	private Mode mode;

	// Use this for initialization
	void Start () {
		blinkTimeSpan = Time.time;
		naturesWrathTimeSpan = Time.time;
		mode = Mode.ARPG;
		globalCooldown = 1f / attackSpeed;
		globalCooldownTimeSpan = Time.time;
		anim = gameObject.GetComponentInChildren<Animator>();
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
		if (globalCooldownTimeSpan <= Time.time) {

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					ElementalMissiles ();
				} else {
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (mode == Mode.ARPG) {
					if (blinkTimeSpan <= Time.time) {
						Blink ();
					}
				} else {
					Grapple();
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				Debug.Log ("Pressed key 3!");
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				Debug.Log ("Pressed key 4!");
				globalCooldownTimeSpan = Time.time + globalCooldown;
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
	}

	void ElementalMissiles() {
		if (currentTarget.tag == "Enemy") {
			moving = false;
			elementalMissiles.GetComponent<MissileMovement>().target = currentTarget;
			elementalMissiles.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z+1);
			RotateTowardsTargetPosition(currentTarget.position);
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

	void Grapple() {
		Instantiate (grapple);
		grapple.GetComponent<GrappleLogic> ().playerPosition = transform.position;
		grapple.GetComponent<GrappleLogic> ().playerRotation = transform.rotation.eulerAngles;
		grapple.position = new Vector3 (transform.position.x + 1, 1, transform.position.z + 1);
	}

	void MouseMovement() {
		if (Input.GetMouseButtonDown (0)) {
			moving = true;
			GetMouseWorldPosition();
		}
		
		if (moving) {
			anim.Play ("Run");
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				moving = false;
			}
		} else {
			anim.Play("Idle");
		}
	}

	void RotateTowardsTargetPosition(Vector3 targetPosition) {
		var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = targetRotation;
		var rotation = transform.rotation.eulerAngles;
		rotation.x = 270;
		rotation.z = 0;
		transform.rotation = Quaternion.Euler (rotation);
	}

	void GetMouseWorldPosition() {
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float hitDist = 0.0f;
		
		if (playerPlane.Raycast (ray, out hitDist)) {	
			var targetPoint = ray.GetPoint(hitDist);
			targetPosition = ray.GetPoint(hitDist);
			RotateTowardsTargetPosition(targetPoint);
		}
	}
}