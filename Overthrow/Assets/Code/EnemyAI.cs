using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	
	public Transform player;
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;
	public static bool isPlayerAlive = true;
	private string hitobject;
	public int maxHealth;
	public int health;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (health < 0) {
			Destroy(gameObject);
		}
		else {
			playerDistance = Vector3.Distance (player.position, transform.position);
			if (playerDistance < 30f){
				//transform.Rotate(0,20*Time.deltaTime,0);
				//transform.rotation = new Quaternion(transform.rotation.x,180,transform.rotation.z,transform.rotation.w);
				lookAtPlayer ();
			}
			if (playerDistance < 25f) { 
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
		//transform.rotation.y += 180;

	}

	void chase(){
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision)
	{
	
	}
	void attack(){
		RaycastHit hit;
		health -= 5;
		Debug.Log("tou aqui");
		if (Physics.Raycast (transform.position, transform.forward ,out hit)){
			Debug.Log("tou aqui1");
			if(hit.collider.gameObject.name == "Teste"){
				Debug.Log("tou aqui2");
				hit.collider.gameObject.GetComponent<PlayerControl>().TakeDamage(2);
			}
		}
	}
}