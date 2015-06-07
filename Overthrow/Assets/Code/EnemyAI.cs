using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public Transform player;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public static bool isPlayerAlive = true;
	private string hitobject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerAlive) {
			playerDistance = Vector3.Distance (player.position, transform.position);

			if (playerDistance < 18f) {
				lookAtPlayer ();
			}
			if (playerDistance < 16f) {
				if (playerDistance > 3f) {
					chase ();
				} 
				else if (playerDistance < 3f) {
					attack ();
				}
			}
		}
	}

	void lookAtPlayer(){
		Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);

	}

	void chase(){
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision)
	{
	
	}
	void attack(){
		RaycastHit hit;
		Debug.Log("tou aqui");
		if (Physics.Raycast (transform.position, transform.forward ,out hit)){
			Debug.Log("tou aqui1");
			if(hit.collider.gameObject.tag == "Player"){
				Debug.Log("tou aqui2");
				hit.collider.gameObject.GetComponent<PlayerControl>().currentHealth -= 20;
			}
		}
	}
}