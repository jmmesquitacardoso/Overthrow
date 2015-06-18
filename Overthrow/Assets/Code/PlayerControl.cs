using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	
	public float speed = 3.0f;
	public float blinkCooldown = 15f;
	public float naturesWrathCooldown = 15f;
	public float grappleCooldown = 15f;
	public float attackSpeed = 1f;
	public float dodgeChance = 15f;
	public float critChance = 15f;
	public float criticalHitDamage = 1;
	public float burnDuration = 3f;
	private float blinkTimeSpan;
	private float naturesWrathTimeSpan;
	private float grappleTimeSpan;
	private float globalCooldown;
	private float globalCooldownTimeSpan;
	private float oldY;
	public int maxHealth = 400;
	public int currentHealth = 200;
	public int healthPerSecond = 2;
	public int maxMana = 400;
	public int currentMana = 200;
	public int manaPerSecond = 2;
	public int maxStrength = 100;
	public int currentStrength = 100;
	public int attackPower = 1000;
	public int blizzardRange = 30;
	public int trapRange = 10;
	public int grappleRange = 40;
	public int blizzardManaCost = 100;
	private Vector3 targetPosition;
	private Animator anim;
	private Animator bowAnim;
	private PlayerState state;
	public Transform elementalMissiles;
	public Transform grapple;
	public Transform trap;
	public Transform flare;
	public Transform naturesWrath;
	public Transform blizzard;
	public Transform bow;
	private Transform currentTarget;
	private Mode mode;
	public Text warningText;
	public bool hoveringShrine = false;
	private bool shiftDown = false;
	private bool blinkBack = false;
	public bool burning = false;
	public Image healthGlobe;
	public Image manaGlobe;
	public Image strengthGlobe;
	public Image missilesIcon;
	public Image blinkIcon;
	public Image blizzardIcon;
	public Image naturesWrathIcon;
	public Image trapIcon;
	public Image grappleIcon;
	public Image flareIcon;
	public Image mindControlIcon;
	private Color blizzardIconColor;
	private Vector3 positionBeforeBlink;
	private ArrayList currentBuffs;
	
	// Use this for initialization
	void Start ()
	{
		oldY = transform.position.y;
		blinkTimeSpan = Time.time;
		naturesWrathTimeSpan = Time.time;
		grappleTimeSpan = Time.time;
		mode = Mode.ARPG;
		globalCooldown = 1f / attackSpeed;
		globalCooldownTimeSpan = Time.time;
		anim = gameObject.GetComponentInChildren<Animator> ();
		bowAnim = GameObject.Find ("Arco").GetComponent<Animator> ();
		state = PlayerState.IDLE;
		blizzardIconColor = blizzardIcon.color;
		currentBuffs = new ArrayList ();
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
		healthGlobe.fillAmount = (float)currentHealth / (float)maxHealth;
		
		manaGlobe.fillAmount = (float)currentMana / (float)maxMana;

		strengthGlobe.fillAmount = (float)currentStrength / (float)maxStrength;

		SpellIconsHandler ();

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			shiftDown = true;
		}

		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			shiftDown = false;
		}

		PlayerSkills ();
		
		UpdateCurrentTarget ();

		/*if (mode == Mode.ARPG) {
			bow.gameObject.SetActive(false);
		} else if (mode == Mode.STEALTH) {
			bow.gameObject.SetActive(true);
		}*/
		
		switch (state) {
		case PlayerState.IDLE:
			anim.Play ("Idle");
			break;
		case PlayerState.MOVING:
			anim.Play ("Run");
			break;
		case PlayerState.BLINK:
			anim.Play ("Blink");
			break;
		case PlayerState.BLIZZNATURE:
			anim.Play ("Blizzard");
			break;
		case PlayerState.GRAPPLE:
			anim.Play("Grapple");
			break;
		case PlayerState.FLARE:
			anim.Play("ShootBow");
			bowAnim.Play("Shoot");
			break;
		default:
			break;
		}
	
		if (currentStrength <= 0 && mode == Mode.STEALTH) {
			ChangeMode ();
		}

		RaycastHit hit;
		Physics.Raycast(transform.position, Vector3.down, out hit);
		if (Physics.Raycast(transform.position, Vector3.down, 2)){
			transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
			this.GetComponent<Rigidbody>().AddForce(transform.up * -20, ForceMode.VelocityChange);
		}

		if (burning) {
			burn();
		}
	}
	
	void FixedUpdate ()
	{
		MouseMovement ();
	}

	void SpellIconsHandler() {
		if (blinkTimeSpan > Time.time) {
			blinkIcon.fillAmount = 1f - ((float)((blinkTimeSpan - Time.time)) / blinkCooldown);
		}
		
		if (naturesWrathTimeSpan > Time.time) {
			naturesWrathIcon.fillAmount = 1f - ((float)((naturesWrathTimeSpan - Time.time)) / naturesWrathCooldown);
		}

		if (grappleTimeSpan > Time.time) {
			grappleIcon.fillAmount = 1f - ((float)((grappleTimeSpan - Time.time)) / grappleCooldown);
		}

		if (currentMana < blizzardManaCost) {
			blizzardIcon.color = Color.Lerp (Color.black, Color.gray, Time.time * 5f);
		} else {
			blizzardIcon.color = blizzardIconColor;
		}
	}

	public void AddBuff(PlayerBuffs buff) {
		bool valid = true;
		for (int i = 0, l = currentBuffs.Count; i < l; i++) {
			if ((PlayerBuffs) currentBuffs[i] == buff) {
				valid = false;
				break;
			}
		}

		if (valid) {
			currentBuffs.Add(buff);
			StartCoroutine(BuffDuration(buff));
		}
	}

	IEnumerator BuffDuration(PlayerBuffs buff) {
		switch (buff) {
		case PlayerBuffs.CRITICAL:
			critChance += 25f;
			break;
		case PlayerBuffs.DESTRUCTION:
			attackPower += 2000;
			break;
		case PlayerBuffs.SWIFT:
			speed += 15;
			break;
		default:
			break;
		}
		yield return new WaitForSeconds (120f);
		switch (buff) {
		case PlayerBuffs.CRITICAL:
			critChance -= 25f;
			break;
		case PlayerBuffs.DESTRUCTION:
			attackPower -= 2000;
			break;
		case PlayerBuffs.SWIFT:
			speed -= 15;
			break;
		default:
			break;
		}
	}

	public void TakeDamage(int damage) {
		if (!Utils.Instance.Dodge(dodgeChance)) {
			currentHealth -= damage;
		}

		if (currentHealth <= 0) {
			EnemyAI.isPlayerAlive = false;
			Destroy (gameObject);
		}
	}
	
	//Displays the current frames per second
	void OnGUI ()
	{
		GUI.Label (new Rect (0, 0, 100, 100), "FPS = " + (int)(1.0f / Time.smoothDeltaTime)); 
	}
	
	void OnCollisionEnter (Collision collision)
	{
		//Debug.Log (collision.gameObject.transform.tag);
		if (collision.gameObject.transform.tag == "Terrain") {
			Vector3 position = transform.position;
			position.y += 0.01f;
			transform.position = position;
		} else if (state == PlayerState.MOVING) {
			state = PlayerState.IDLE;
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

	IEnumerator DisplayWarningText (string warning)
	{
		warningText.text = warning;
		yield return new WaitForSeconds (2f);
		warningText.text = "";
	}
	
	//Function that reacts to the current key down and casts the skill associated with the key
	void PlayerSkills ()
	{
		//if the global cooldown is over
		if (globalCooldownTimeSpan <= Time.time) {
			
			if (!shiftDown && Input.GetKeyDown (KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					ElementalMissiles (currentTarget.position, true);
				} else {
					Trap ();
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (shiftDown && Input.GetKeyDown (KeyCode.Alpha1)) {
				if (mode == Mode.ARPG) {
					GetMouseWorldPosition ();
					ElementalMissiles (targetPosition, false);
				} else {
					
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (!blinkBack && Input.GetKeyDown (KeyCode.Alpha2)) {
				if (mode == Mode.ARPG) {
					//if blink is not on cooldown
					if (blinkTimeSpan <= Time.time) {
						StartCoroutine(Blink());
					} else {
						StartCoroutine (DisplayWarningText ("Blink is on cooldown!"));
					}
				} else {
					if (grappleTimeSpan <= Time.time) {
						StartCoroutine(Grapple());
					} else {
						StartCoroutine (DisplayWarningText ("Grapple is on cooldown!"));
					}
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			} else if (blinkBack && Input.GetKeyDown (KeyCode.Alpha2)) {
				if (mode == Mode.ARPG) {
					//if blink is not on cooldown
					if (blinkTimeSpan <= Time.time) {
						transform.position = positionBeforeBlink;
						blinkBack = false;
					} else {
						StartCoroutine (DisplayWarningText ("Blink is on cooldown!"));
					}
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				if (mode == Mode.ARPG) {
					StartCoroutine(Blizzard());
				} else {
					StartCoroutine(Flare());
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				if (mode == Mode.ARPG) {
					if (naturesWrathTimeSpan <= Time.time) {
						StartCoroutine(NaturesWrath());
					} else {
						StartCoroutine (DisplayWarningText ("Nature's Wrath is on cooldown!"));
					}
				}
				globalCooldownTimeSpan = Time.time + globalCooldown;
			}
			
			if (Input.GetMouseButtonDown (1)) {
				ChangeMode ();
			}
		} else {
			//StartCoroutine(DisplayWarningText("Not ready yet!"));
		}
	}

	// Changes the mode, and changes the icons and the power globe accordingly
	void ChangeMode ()
	{
		if (mode == Mode.ARPG) {
			mode = Mode.STEALTH;
			manaGlobe.enabled = false;
			strengthGlobe.enabled = true;
			missilesIcon.enabled = false;
			blinkIcon.enabled = false;
			blizzardIcon.enabled = false;
			naturesWrathIcon.enabled = false;
			trapIcon.enabled = true;
			grappleIcon.enabled = true;
			flareIcon.enabled = true;
			mindControlIcon.enabled = true;
		} else {
			mode = Mode.ARPG;
			manaGlobe.enabled = true;
			strengthGlobe.enabled = false;
			missilesIcon.enabled = true;
			blinkIcon.enabled = true;
			blizzardIcon.enabled = true;
			naturesWrathIcon.enabled = true;
			trapIcon.enabled = false;
			grappleIcon.enabled = false;
			flareIcon.enabled = false;
			mindControlIcon.enabled = false;
		}
	}
	
	// Casts the Elemental Missiles skill
	// Player stops moving and rotates in the direction of the missiles
	void ElementalMissiles (Vector3 targetPosition, bool targeted)
	{
		if (currentTarget != null && currentTarget.tag == "Enemy" && targeted || !targeted) {
			state = PlayerState.IDLE;
			elementalMissiles.GetComponent<MissileLogic> ().targetPosition = new Vector3 (targetPosition.x, targetPosition.y + 3, targetPosition.z);
			elementalMissiles.GetComponent<MissileLogic> ().damage = (int)(attackPower * 0.10);
			elementalMissiles.GetComponent<MissileLogic> ().critChance = critChance;
			elementalMissiles.GetComponent<MissileLogic> ().criticalHitDamage = criticalHitDamage;
			elementalMissiles.position = new Vector3 (transform.position.x + 1, transform.position.y+3, transform.position.z + 1);
			RotateTowardsTargetPosition (targetPosition);
			Instantiate (elementalMissiles);
		}
	}
	
	//Casts the Trap skill
	void Trap ()
	{
		state = PlayerState.IDLE;
		GetMouseWorldPosition ();
		if (Vector3.Distance (targetPosition, transform.position) <= trapRange) {
			trap.position = targetPosition;
			var position = trap.position;
			position.y = 1.5f;
			trap.position = position;
			Instantiate (trap);
		} else {
			StartCoroutine (DisplayWarningText ("Out of range!"));
		}
	}

	// Casts the Flare skill
	IEnumerator Flare ()
	{
		anim.speed = 2;
		state = PlayerState.FLARE;
		GetMouseWorldPosition ();
		flare.position = new Vector3 (transform.position.x + Mathf.Cos (transform.rotation.eulerAngles.y), 1, transform.position.z + Mathf.Sin (transform.rotation.eulerAngles.y));
		flare.GetComponent<FlareLogic> ().targetPosition = targetPosition;
		flare.GetComponent<FlareLogic> ().rotation = transform.rotation.eulerAngles;
		yield return new WaitForSeconds(1f);
		Instantiate (flare);
		anim.speed = 1;
		state = PlayerState.IDLE;
	}

	// Casts the Blizzard skill
	IEnumerator Blizzard ()
	{
		GetMouseWorldPosition ();
		if (Vector3.Distance (targetPosition, transform.position) <= blizzardRange) {
			if (currentMana >= blizzardManaCost) {
				anim.speed = 2.5f;
				state = PlayerState.BLIZZNATURE;
				blizzard.GetComponent<BlizzardLogic> ().damage = (int)(attackPower * 0.01);
				blizzard.GetComponent<BlizzardLogic> ().critChance = critChance;
				blizzard.GetComponent<BlizzardLogic> ().criticalHitDamage = criticalHitDamage;
				Vector3 blizzardPosition = targetPosition;
				blizzardPosition.y = 15.5f;
				blizzard.position = blizzardPosition;
				yield return new WaitForSeconds(1f);
				Instantiate (blizzard);
				currentMana -= blizzardManaCost;
				anim.speed = 1;
				state = PlayerState.IDLE;
			} else {
				StartCoroutine (DisplayWarningText ("Not enough mana!"));
			}
		} else {
			StartCoroutine (DisplayWarningText ("Out of range!"));
		}
		yield return null;
	}
	
	//Casts the Blink skill
	IEnumerator Blink ()
	{
		positionBeforeBlink = transform.position;
		blinkBack = true;
		GetMouseWorldPosition ();
		transform.position = targetPosition;
		//state = PlayerState.BLINK;
		yield return new WaitForSeconds (3f);
		blinkBack = false;
		blinkTimeSpan = Time.time + blinkCooldown;
		//state = PlayerState.IDLE;
	}
	
	//Casts the Grapple skill
	IEnumerator Grapple ()
	{
		GetMouseWorldPosition ();
		if (Vector3.Distance (targetPosition, transform.position) <= grappleRange) {
			state = PlayerState.GRAPPLE;
			anim.speed = 2.5f;
			grapple.position = new Vector3 (targetPosition.x, 1, targetPosition.z);
			yield return new WaitForSeconds(1.5f);
			grappleTimeSpan = Time.time + grappleCooldown;
			anim.speed = 1;
			Instantiate (grapple);
			state = PlayerState.IDLE;
		}
	}

	//Casts the Nature's Wrath skill
	IEnumerator NaturesWrath ()
	{
		anim.speed = 2.5f;
		state = PlayerState.BLIZZNATURE;
		GetMouseWorldPosition ();
		naturesWrathTimeSpan = Time.time + naturesWrathCooldown;
		naturesWrath.GetComponent<NaturesWrathLogic> ().targetPosition = targetPosition;
		naturesWrath.GetComponent<NaturesWrathLogic> ().damage = (int)(attackPower * 0.10);
		naturesWrath.GetComponent<NaturesWrathLogic> ().critChance = critChance;
		naturesWrath.GetComponent<NaturesWrathLogic> ().criticalHitDamage = criticalHitDamage;
		naturesWrath.position = new Vector3 (transform.position.x + Mathf.Cos (transform.rotation.eulerAngles.y), transform.position.y, transform.position.z + Mathf.Sin (transform.rotation.eulerAngles.y));
		yield return new WaitForSeconds (1f);
		Instantiate (naturesWrath);
		anim.speed = 1f;
		state = PlayerState.IDLE;
	}
	
	//Handles the movement based on mouse clicks
	void MouseMovement ()
	{
		if ((Input.GetMouseButtonDown (0) && state == PlayerState.IDLE && !hoveringShrine) || (Input.GetMouseButtonDown (0) && state == PlayerState.MOVING && !hoveringShrine)) {
			state = PlayerState.MOVING;
			GetMouseWorldPosition ();
		}

		if (state == PlayerState.MOVING) {

			transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
			if ((targetPosition - transform.position).magnitude < 0.1) {
				state = PlayerState.IDLE;
			}

		}
	}
	
	//Rotates the player in the direction of the vector3 targetPosition
	void RotateTowardsTargetPosition (Vector3 targetPosition)
	{
		var targetRotation = Quaternion.LookRotation (targetPosition - transform.position);
		transform.rotation = targetRotation;
		var rotation = transform.rotation.eulerAngles;
		rotation.x = 0;
		rotation.y += 180;
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

	public void StartBurning(float duration) {
		burning = true;
		burnDuration = duration;
	}

	private float burnDamage = 0;
	private void burn() {
		burnDuration -= Time.deltaTime;
		currentHealth -= 1;
		if (burnDuration <= 0) {
			burning = false;

		}
	}
}