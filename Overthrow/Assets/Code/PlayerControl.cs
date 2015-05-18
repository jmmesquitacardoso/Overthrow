using UnityEngine;
using System.Collections;

enum Mode {ARPG, Stealth};
enum PlayerState {Idle, Moving};

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

	public int maxHealth = 400;
	public int currentHealth = 400;
	public int healthPerSecond = 2;
	public int maxMana = 400;
	public int currentMana = 200;
	public int manaPerSecond = 2;
	public int strength = 100;
	public int attackPower = 1000;
	public int criticalHitDamage = 1;

	private Vector3 targetPosition; 

	private Animator anim;

	private PlayerState state;
	
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
		state = PlayerState.Idle;
		InvokeRepeating("ManaRegen",0,1f);
	}

	void Awake() {
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {

		PlayerSkills ();

		UpdateCurrentTarget ();

		switch (state) {
		case PlayerState.Idle: anim.Play("Idle");
			break;
		case PlayerState.Moving: anim.Play("Run");
			break;
		default: break;
		}

	}

	void FixedUpdate () {
		
		MouseMovement ();
	}

	//Displays the current frames per second
	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 100), "FPS = " + (int)(1.0f / Time.smoothDeltaTime));     
		GUI.Label(new Rect(100, 0, 150, 100), "MANA = " + currentMana);  
	}

	void OnCollisionEnter(Collision collision) {
		if (state == PlayerState.Moving) {
			state = PlayerState.Idle;
		}
	}

	//This function is called every second, so the mana regens at a rate of manaPerSecond/second
	void ManaRegen() {
		if (currentMana <= maxMana) {
			if ((currentMana+manaPerSecond) <= maxMana) {
				currentMana += manaPerSecond;
			} else {
				currentMana = maxMana;
			}
		}
	}

	// Gets the current target the mouse is hovering
	void UpdateCurrentTarget() {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
	    RaycastHit hit;

		if(Physics.Raycast(ray, out hit))
		{
			currentTarget = hit.transform;
		}
	}

	//Function that reacts to the current key down and casts the skill associated with the key
	void PlayerSkills() {
		//if the global cooldown is over
		if (globalCooldownTimeSpan <= Time.time) {

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					ElementalMissiles (currentTarget.position);
				} else {
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}

			if (Input.GetKeyDown(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.LeftShift)) {
				if (mode == Mode.ARPG) {
					ElementalMissiles (Vector3.zero);
				} else {
				}
			}
		
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (mode == Mode.ARPG) {
					//if blink is not on cooldown
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

	// Casts the Elemental Missiles skill
	// Player stops moving and rotates in the direction of the missiles
	void ElementalMissiles(Vector3 targetPosition) {
		if (currentTarget.tag == "Enemy") {
			state = PlayerState.Idle;
			elementalMissiles.GetComponent<MissileMovement>().targetPosition = targetPosition;
			elementalMissiles.position = new Vector3(transform.position.x+1,transform.position.y,transform.position.z+1);
			RotateTowardsTargetPosition(currentTarget.position);
			Instantiate(elementalMissiles);
		}
	}

	//Casts the Blink skill
	void Blink() {
		state = PlayerState.Idle;
		GetMouseWorldPosition();
		transform.position = targetPosition;
		blinkTimeSpan = Time.time + blinkCooldown;
	}

	//Casts the Grapple skill
	void Grapple() {
		state = PlayerState.Idle;
		grapple.GetComponent<GrappleLogic> ().playerPosition = transform.position;
		GetMouseWorldPosition ();
		grapple.GetComponent<GrappleLogic> ().targetPosition = targetPosition;
		RotateTowardsTargetPosition (targetPosition);
		grapple.GetComponent<GrappleLogic> ().playerRotation = transform.rotation.eulerAngles;
		grapple.position = new Vector3 (transform.position.x + 1, 1, transform.position.z + 1);
		Instantiate (grapple);
	}

	//Handles the movement based on mouse clicks
	void MouseMovement() {
		if (Input.GetMouseButtonDown (0)) {
			state = PlayerState.Moving;
			GetMouseWorldPosition();
		}
		
		if (state == PlayerState.Moving) {
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				state = PlayerState.Idle;
			}
		}
	}

	//Rotates the player in the direction of the vector3 targetPosition
	void RotateTowardsTargetPosition(Vector3 targetPosition) {
		var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = targetRotation;
		var rotation = transform.rotation.eulerAngles;
		rotation.x = 270;
		rotation.z = 0;
		transform.rotation = Quaternion.Euler (rotation);
	}

	//Gets the world coordinates of the mouse
	void GetMouseWorldPosition() {
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float hitDist = 0.0f;
		
		if (playerPlane.Raycast (ray, out hitDist)) {
			targetPosition = ray.GetPoint(hitDist);
			RotateTowardsTargetPosition(targetPosition);
		}
	}
}