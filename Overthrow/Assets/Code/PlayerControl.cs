using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	
	public float speed = 3.0f;
	public float blinkCooldown = 15f;
	public float naturesWrathCooldown = 15f;
	public float attackSpeed = 1f;
	public float dodgeChance = 15f;
	public float critChance = 15f;
	public float criticalHitDamage = 1;
	private float blinkTimeSpan;
	private float naturesWrathTimeSpan;
	private float globalCooldown;
	private float globalCooldownTimeSpan;
	public int maxHealth = 400;
	public int currentHealth = 200;
	public int healthPerSecond = 2;
	public int maxMana = 400;
	public int currentMana = 200;
	public int manaPerSecond = 2;
	public int strength = 100;
	public int attackPower = 1000;
	public int blizzardRange = 30;
	public int trapRange = 10;
	private Vector3 targetPosition;
	private Animator anim;
	private PlayerState state;
	public Transform elementalMissiles;
	public Transform grapple;
	public Transform trap;
	public Transform flare;
	public Transform naturesWrath;
	public Transform blizzard;
	private Transform currentTarget;
	private Mode mode;
	public Text warningText;
	private bool shiftDown = false;
	public Image healthGlobe;
	public Image manaGlobe;
	public Image blinkIcon;
	public Image naturesWrathIcon;
	
	// Use this for initialization
	void Start ()
	{
		blinkTimeSpan = Time.time;
		naturesWrathTimeSpan = Time.time;
		mode = Mode.ARPG;
		globalCooldown = 1f / attackSpeed;
		globalCooldownTimeSpan = Time.time;
		anim = gameObject.GetComponentInChildren<Animator> ();
		state = PlayerState.Idle;
		InvokeRepeating ("ManaRegen", 0, 1f);
		InvokeRepeating ("HealthRegen", 0, 1f);
	}
	
	void Awake ()
	{
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update ()
	{
		healthGlobe.fillAmount = (float) currentHealth / (float) maxHealth;

		manaGlobe.fillAmount = (float) currentMana / (float) maxMana;

		if (blinkTimeSpan > Time.time) {
			Debug.Log("Diff: " + ((float) ((blinkTimeSpan - Time.time))) / blinkCooldown);
			blinkIcon.fillAmount = 1f - ((float) ((blinkTimeSpan - Time.time)) / blinkCooldown);
		}

		if (naturesWrathTimeSpan > Time.time) {
			Debug.Log("Diff: " + ((float) ((naturesWrathTimeSpan - Time.time))) / naturesWrathCooldown);
			naturesWrathIcon.fillAmount = 1f - ((float) ((naturesWrathTimeSpan - Time.time)) / naturesWrathCooldown);
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			shiftDown = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			shiftDown = false;
		}

		PlayerSkills ();
		
		UpdateCurrentTarget ();
		
		switch (state) {
		case PlayerState.Idle:
			anim.Play ("Idle");
			break;
		case PlayerState.Moving:
			anim.Play ("Run");
			break;
		default:
			break;
		}
		
	}
	
	void FixedUpdate ()
	{
		
		MouseMovement ();
	}
	
	//Displays the current frames per second
	void OnGUI ()
	{
		GUI.Label (new Rect (0, 0, 100, 100), "FPS = " + (int)(1.0f / Time.smoothDeltaTime));     
		GUI.Label (new Rect (100, 0, 150, 100), "MANA = " + currentMana);    
		GUI.Label (new Rect (250, 0, 150, 100), "Health = " + currentHealth);  
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (state == PlayerState.Moving) {
			state = PlayerState.Idle;
		}
	}
	
	//This function is called every second, so the mana regens at a rate of manaPerSecond/second
	void ManaRegen ()
	{
		if (currentMana <= maxMana) {
			if ((currentMana + manaPerSecond) <= maxMana) {
				currentMana += manaPerSecond;
			} else {
				currentMana = maxMana;
			}
		}
	}
	
	//This function is called every second, so health regens at a rate of healthPerSecond/second
	void HealthRegen ()
	{
		if (currentHealth <= maxHealth) {
			if ((currentHealth + healthPerSecond) <= maxHealth) {
				currentHealth += healthPerSecond;
			} else {
				currentHealth = maxHealth;
			}
		}
	}
	
	// Gets the current target the mouse is hovering
	void UpdateCurrentTarget ()
	{
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit)) {
			currentTarget = hit.transform;
		}
	}

	IEnumerator DisplayWarningText(string warning) {
		warningText.text = warning;
		yield return new WaitForSeconds (2f);
		warningText.text = "";
	}
	
	//Function that reacts to the current key down and casts the skill associated with the key
	void PlayerSkills ()
	{
		//if the global cooldown is over
		if (globalCooldownTimeSpan <= Time.time) {
			
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					ElementalMissiles (currentTarget.position, true);
				} else {
					Trap ();
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (shiftDown && Input.GetKeyDown(KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					GetMouseWorldPosition();
					ElementalMissiles (targetPosition, false);
				} else {
					
				}
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (mode == Mode.ARPG) {
					//if blink is not on cooldown
					if (blinkTimeSpan <= Time.time) {
						Blink ();
					} else {
						StartCoroutine(DisplayWarningText("Blink is on cooldown!"));
					}
				} else {
					Grapple ();
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				if (mode == Mode.ARPG) {
					Blizzard ();
				} else {
					Flare ();
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				if (mode == Mode.ARPG) {
					if (naturesWrathTimeSpan <= Time.time) {
						NaturesWrath ();
					} else {
						StartCoroutine(DisplayWarningText("Nature's Wrath is on cooldown!"));
					}
				}
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
		} else {
			//StartCoroutine(DisplayWarningText("Not ready yet!"));
		}
	}
	
	// Casts the Elemental Missiles skill
	// Player stops moving and rotates in the direction of the missiles
	void ElementalMissiles (Vector3 targetPosition, bool targeted)
	{
		if (currentTarget.tag == "Enemy" && targeted || !targeted) {
			state = PlayerState.Idle;
			elementalMissiles.GetComponent<MissileLogic> ().targetPosition = targetPosition;
			elementalMissiles.GetComponent<MissileLogic> ().damage = (int)(attackPower * 0.10);
			elementalMissiles.GetComponent<MissileLogic> ().critChance = critChance;
			elementalMissiles.GetComponent<MissileLogic> ().criticalHitDamage = criticalHitDamage;
			elementalMissiles.position = new Vector3 (transform.position.x + 1, transform.position.y, transform.position.z + 1);
			RotateTowardsTargetPosition (targetPosition);
			Instantiate (elementalMissiles);
		}
	}
	
	//Casts the Trap skill
	void Trap ()
	{
		state = PlayerState.Idle;
		GetMouseWorldPosition ();
		if (Vector3.Distance (targetPosition, transform.position) <= trapRange) {
			trap.position = targetPosition;
			var position = trap.position;
			position.y = 1.5f;
			trap.position = position;
			Instantiate (trap);
		} else {
			StartCoroutine(DisplayWarningText("Out of range!"));
		}
	}

	//casts the flare skill
	void Flare() {
		state = PlayerState.Idle;
		GetMouseWorldPosition ();
		flare.position = new Vector3 (transform.position.x + Mathf.Cos (transform.rotation.eulerAngles.y), 1, transform.position.z + Mathf.Sin (transform.rotation.eulerAngles.y));
		flare.GetComponent<Flare>().targetPosition = targetPosition;
		flare.GetComponent<Flare> ().rotation = transform.rotation.eulerAngles;
		Instantiate (flare);
	}

	void Blizzard ()
	{
		state = PlayerState.Idle;
		GetMouseWorldPosition ();
		if (Vector3.Distance (targetPosition, transform.position) <= blizzardRange) {
			blizzard.GetComponent<BlizzardLogic> ().damage = (int)(attackPower * 0.10);
			blizzard.GetComponent<BlizzardLogic> ().critChance = critChance;
			blizzard.GetComponent<BlizzardLogic> ().criticalHitDamage = criticalHitDamage;
			Vector3 blizzardPosition = targetPosition;
			blizzardPosition.y = 15.5f;
			blizzard.position = blizzardPosition;
			Instantiate (blizzard);
		} else {
			StartCoroutine(DisplayWarningText("Out of range!"));
		}
	}
	
	//Casts the Blink skill
	void Blink ()
	{
		blinkTimeSpan = Time.time + blinkCooldown;
		state = PlayerState.Idle;
		GetMouseWorldPosition ();
		transform.position = targetPosition;
	}
	
	//Casts the Grapple skill
	void Grapple ()
	{
		state = PlayerState.Idle;
		grapple.GetComponent<GrappleLogic> ().playerPosition = transform.position;
		GetMouseWorldPosition ();
		grapple.GetComponent<GrappleLogic> ().targetPosition = targetPosition;
		grapple.GetComponent<GrappleLogic> ().playerRotation = transform.rotation.eulerAngles;
		grapple.position = new Vector3 (transform.position.x + Mathf.Cos (transform.rotation.eulerAngles.y), 1, transform.position.z + Mathf.Sin (transform.rotation.eulerAngles.y));
		Instantiate (grapple);
	}
	
	void NaturesWrath ()
	{
		naturesWrathTimeSpan = Time.time + naturesWrathCooldown;
		state = PlayerState.Idle;
		GetMouseWorldPosition ();
		naturesWrath.GetComponent<NaturesWrathLogic> ().targetPosition = targetPosition;
		naturesWrath.GetComponent<NaturesWrathLogic> ().damage = (int)(attackPower * 0.10);
		naturesWrath.GetComponent<NaturesWrathLogic> ().critChance = critChance;
		naturesWrath.GetComponent<NaturesWrathLogic> ().criticalHitDamage = criticalHitDamage;
		naturesWrath.position = new Vector3 (transform.position.x + Mathf.Cos (transform.rotation.eulerAngles.y), transform.position.y, transform.position.z + Mathf.Sin (transform.rotation.eulerAngles.y));
		Instantiate (naturesWrath);
	}
	
	//Handles the movement based on mouse clicks
	void MouseMovement ()
	{
		if (Input.GetMouseButtonDown (0)) {
			state = PlayerState.Moving;
			GetMouseWorldPosition ();
		}
		
		if (state == PlayerState.Moving) {
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				state = PlayerState.Idle;
			}
		}
	}
	
	//Rotates the player in the direction of the vector3 targetPosition
	void RotateTowardsTargetPosition (Vector3 targetPosition)
	{
		var targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
		transform.rotation = targetRotation;
		var rotation = transform.rotation.eulerAngles;
		rotation.x = 270;
		rotation.z = 0;
		transform.rotation = Quaternion.Euler (rotation);
	}
	
	//Gets the world coordinates of the mouse
	void GetMouseWorldPosition ()
	{
		Plane playerPlane = new Plane (Vector3.up, transform.position);
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float hitDist = 0.0f;
		
		if (playerPlane.Raycast (ray, out hitDist)) {
			targetPosition = ray.GetPoint (hitDist);
			RotateTowardsTargetPosition (targetPosition);
		}
	}
}