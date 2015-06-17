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

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		playerDistance = Vector3.Distance (player.position, transform.position);

		if (playerDistance < 90f) {
			//transform.Rotate(0,20*Time.deltaTime,0);
			//transform.rotation = new Quaternion(transform.rotation.x,180,transform.rotation.z,transform.rotation.w);
			lookAtPlayer ();
		}

		if (playerDistance < 85f) { 
			if (playerDistance > 3f) {
				chase ();
			} else if (playerDistance < 3f) {
				attack ();
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
}