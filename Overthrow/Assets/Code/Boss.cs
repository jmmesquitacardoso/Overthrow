using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	
	public float attackSpeed = 10f;
	public float critChance = 15f;
	public float criticalHitDamage = 1;
	public float moveSpeed = 6;
	public float rotationDamping = 15;
	public int attackPower = 3000;
	public int MaxHealth=2000;
	public int Health{ get; private set; }
	public Transform rain;
	public Transform fire;
	public Transform elementalMissiles;
	public Transform playerTarget;

	public float playerDistance;
	public float fireRate = 20;
	public float canFire;
	// Use this for initialization
	public void Awake () {
		Health = MaxHealth;
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		playerDistance = Vector3.Distance (playerTarget.position, transform.position);
		if (playerDistance < 80f) {
			lookAtPlayer ();
		
			if (playerDistance < 60f && playerDistance > 6f ) {
				transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		
				if (playerDistance < 55f) {
					if ((canFire -= Time.deltaTime) > 0)
						return;
					ElementalMissiles (playerTarget.position, true);
					canFire = fireRate;
				}
			}
		}
	}

	void lookAtPlayer ()
	{
		Quaternion rotation = Quaternion.LookRotation (playerTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
	}

	void ElementalMissiles (Vector3 targetPosition, bool targeted)
	{
			elementalMissiles.GetComponent<MissileLogic> ().targetPosition = new Vector3 (targetPosition.x, targetPosition.y + 3, targetPosition.z);
			elementalMissiles.GetComponent<MissileLogic> ().damage = (int)(attackPower * 0.10);
			elementalMissiles.GetComponent<MissileLogic> ().critChance = critChance;
			elementalMissiles.GetComponent<MissileLogic> ().criticalHitDamage = criticalHitDamage;
			elementalMissiles.position = new Vector3 (transform.position.x + 1, transform.position.y+3, transform.position.z + 1);
			RotateTowardsTargetPosition (targetPosition);
			Instantiate (elementalMissiles);
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
}
