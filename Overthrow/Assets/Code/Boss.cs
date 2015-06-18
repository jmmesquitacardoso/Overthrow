using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	
	public float attackSpeed = 10f;
	public float critChance = 15f;
	public float criticalHitDamage = 1;
	public float moveSpeed = 6;
	public float rotationDamping = 15;
	public int attackPower = 1200;
	public int MaxHealth=2000;
	public int Health{ get; private set; }
	public Transform fire;
	public Transform elementalMissiles;
	public Transform playerTarget;

	public float playerDistance;
	public float fireRate = 1;
	public float canFire;
	private Transform fireTransform;
	private float fireTime=2;
	private float fireT;
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
		
			if (playerDistance < 60f && playerDistance > 4f ) {
				transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		
				if (playerDistance < 55f && playerDistance > 12f) {
					if ((canFire -= Time.deltaTime) > 0)
						return;
					ElementalMissiles (playerTarget.position, true);
					canFire = fireRate;
				}
				if(playerDistance < 12f){
					if((fireT -= Time.deltaTime) >0)
						return;
					/*if(fireTransform != null){
						DestroyImmediate(fireTransform.gameObject, true);

					}*/
					fireTransform= Instantiate (fire);
					fireTransform.position = playerTarget.position;
					fireT=fireTime;
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
		EnemyMissileLogic eml = elementalMissiles.GetComponent<EnemyMissileLogic> ();
		eml.targetPosition = new Vector3 (targetPosition.x, targetPosition.y + 3, targetPosition.z);
		eml.damage = (int)(attackPower * 0.10);
		eml.critChance = critChance;
		eml.criticalHitDamage = criticalHitDamage;
		elementalMissiles.position = new Vector3 (transform.position.x + 3, transform.position.y+8, transform.position.z + 1);
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
