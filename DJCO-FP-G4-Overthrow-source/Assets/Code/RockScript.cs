using UnityEngine;
using System.Collections;

public class RockScript : MonoBehaviour {

	public Vector3 playerPosition;
	public float speed = 10f;
	public int damage = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, playerPosition, Time.deltaTime * speed);
		if ((playerPosition - transform.position).magnitude < 0.1) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<PlayerControl>().TakeDamage(damage);
			Destroy(gameObject);
		}
	}
}