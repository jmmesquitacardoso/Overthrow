using UnityEngine;
using System.Collections;

public class NaturesWrathLogic : MonoBehaviour {

	public Vector3 targetPosition;
	
	public float speed = 3.0f;
	public float critChance;
	public float criticalHitDamage;
	
	public int damage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		if ((targetPosition - transform.position).magnitude < 0.1) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<EnemyScript> ().TakeDamage (Utils.Instance.CalculateDamage (critChance, criticalHitDamage, damage));
			collision.gameObject.GetComponent<EnemyScript> ().KnockUp ();
		} else if (collision.gameObject.tag == "Boss") {
			collision.gameObject.GetComponent<Boss> ().TakeDamage (Utils.Instance.CalculateDamage (critChance, criticalHitDamage, damage));
		}
	}
}
