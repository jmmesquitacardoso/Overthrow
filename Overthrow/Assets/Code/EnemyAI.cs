using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	
	public Transform player;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public static bool isPlayerAlive = true;
	private string hitobject;
	private SphereCollider sphereCollider;

	// Use this for initialization
	void Start ()
	{
		sphereCollider = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		playerDistance = Vector3.Distance (player.position, transform.position);


		if (playerDistance < 5f) {
			//transform.Rotate(0,20*Time.deltaTime,0);
			//transform.rotation = new Quaternion(transform.rotation.x,180,transform.rotation.z,transform.rotation.w);
			//lookAtPlayer ();
		}

		if (playerDistance < 85f) { 
			//lookAtPlayer ();
			if (playerDistance > 3f && playerInSight) {
				chase ();
				lookAtPlayer();
			} else if (playerDistance < 3f) {
				attack ();
				lookAtPlayer();
			}
		}
	}

	void lookAtPlayer ()
	{
		Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
	}

	void chase ()
	{
		gameObject.GetComponent<EnemyScript> ().state = EnemyState.WALKING;
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
	}

	void OnCollisionEnter (Collision collision)
	{
	
	}

	void attack ()
	{
		gameObject.GetComponent<EnemyScript> ().state = EnemyState.MELEEATTACKING;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			if (hit.collider.gameObject.name == "Player") {
				hit.collider.gameObject.GetComponent<PlayerControl> ().TakeDamage (1);
			}
		}
	}

	bool playerInSight = false;
	public float fieldOfViewAngle = 110;

	void OnTriggerStay (Collider other)
	{
		// If the player has entered the trigger sphere...
		if(other.gameObject == player.gameObject)
		{

			// By default the player is not in sight.
			playerInSight = false;
			
			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sphereCollider.radius))
				{
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject == player.gameObject)
					{
						// ... the player is in sight.
						playerInSight = true;

					}
				}
			}
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
			// ... the player is not in sight.
			playerInSight = false;
	}
}