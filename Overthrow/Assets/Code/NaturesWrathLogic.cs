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
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy") {
			var random = Random.Range (1,100);
			// If it's an integer crit chance, i.e 15%
			if (Mathf.Floor(critChance) == critChance) {
				if (random <= critChance) {
					collision.gameObject.GetComponent<EnemyScript>().TakeDamage((int)(damage*criticalHitDamage));
				} else {
					collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
				}
			} else { // If it's a float crit chance, i.e 15.7%
				int percentileCritChance = (int)Mathf.Ceil (((critChance - Mathf.Floor(critChance))) * 100);
				var percentileRandom = Random.Range(1,100);
				if (percentileRandom <= percentileCritChance) {
					random += 1;
					if (random <= critChance) {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage((int)(damage*criticalHitDamage));
					} else {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
					}
				} else {
					if (random <= critChance) {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage((int)(damage*criticalHitDamage));
					} else {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
					}
				}
			}
			Destroy(gameObject);
		}
	}
}
