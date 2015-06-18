using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	
	public Transform player;
	public Transform rock;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public float fieldOfViewAngle = 110;
	public static bool isPlayerAlive = true;
	public bool attacking = false;
	public bool ranged = false;
	private string hitobject;
	public int enemyDamage = 2;
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
			lookAtPlayer ();
		}

		if (playerDistance < 85f) { 
			lookAtPlayer ();
			if (ranged) {
				if (playerDistance > 50f) {
					Debug.Log("asd");
					chase ();
					attacking = false;
				} else {
					Debug.Log("asd2");
					if (!attacking) {
						attacking = true;
						StartCoroutine(Attack ());
					}
				}
			} else {
				if (playerDistance > 4f) {
					chase ();
					attacking = false;
				} else {
					if (!attacking) {
						attacking = true;
						StartCoroutine(Attack ());
					}
				} 
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

	IEnumerator Attack ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {
			if (hit.collider.gameObject.name == "Player") {
				if (attacking) {
					if (ranged) {
						rock.GetComponent<RockScript>().transform.position = new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
						rock.GetComponent<RockScript>().playerPosition = new Vector3 (hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y+2.5f, hit.collider.gameObject.transform.position.z);
						if (gameObject.GetComponent<EnemyScript> ().state != EnemyState.RANGEDATTACKING) {
							gameObject.GetComponent<EnemyScript> ().state = EnemyState.RANGEDATTACKING;
						}
						yield return new WaitForSeconds (2.583f);
						Instantiate(rock);
						StartCoroutine(Attack());
					} else {
						if (gameObject.GetComponent<EnemyScript> ().state != EnemyState.MELEEATTACKING) {
							gameObject.GetComponent<EnemyScript> ().state = EnemyState.MELEEATTACKING;
						}
						player.GetComponent<PlayerControl> ().TakeDamage (enemyDamage);
						yield return new WaitForSeconds (2f);
						StartCoroutine(Attack());
					}
				}
			}
		}
	}

	/*void OnTriggerStay (Collider other)
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
	}*/
}